using CGym.Application.Interfaces;
using CGym.Domain.Entities;

namespace CGym.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IMemberRepository _memberRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IActivityRepository activityRepository,
            IMemberRepository memberRepository)
        {
            _bookingRepository = bookingRepository;
            _activityRepository = activityRepository;
            _memberRepository = memberRepository;
        }

        public async Task<Booking> CreateBookingAsync(int memberId, int activityId)
        {
            if (memberId <= 0 || activityId <= 0)
                throw new ArgumentException("MemberId og ActivityId skal være større end 0.");

            var member = await _memberRepository.GetByIdAsync(memberId);
            if (member == null)
                throw new InvalidOperationException("Member not found.");

            var activity = await _activityRepository.GetByIdAsync(activityId);
            if (activity == null)
                throw new InvalidOperationException("Activity not found.");

            var alreadyBooked = await _bookingRepository.ExistsAsync(memberId, activityId);
            if (alreadyBooked)
                throw new InvalidOperationException("Member has already booked this activity.");

            var bookingCount = await _bookingRepository.CountByActivityIdAsync(activityId);
            if (bookingCount >= activity.Capacity)
                throw new InvalidOperationException("Activity is full.");

            var booking = new Booking
            {
                MemberId = memberId,
                ActivityId = activityId,
                BookingDate = DateTime.UtcNow
            };

            await _bookingRepository.AddAsync(booking);

            return booking;
        }
    }
}