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
            var member = await _memberRepository.GetByIdAsync(memberId);

            if (member == null)
                throw new KeyNotFoundException("Member not found");

            var activity = await _activityRepository.GetByIdAsync(activityId);

            if (activity == null)
                throw new Exception("Activity not found");

            var booking = new Booking
            {
                MemberId = memberId,
                ActivityId = activityId,
                BookingDate = DateTime.UtcNow
            };

            await _bookingRepository.AddAsync(booking);

            return booking;
        }

        public async Task<IEnumerable<Booking>> GetBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task DeleteBookingAsync(int id)
        {
            var existing = await _bookingRepository.GetByIdAsync(id);
                if (existing == null)

                    throw new KeyNotFoundException("Booking Not found");
                
                await _bookingRepository.DeleteAsync(id);
        }
    }
}