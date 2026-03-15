using CGym.Application.Interfaces;
using CGym.Domain.Entities;

namespace CGym.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IActivityRepository _activityRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IActivityRepository activityRepository)
        {
            _bookingRepository = bookingRepository;
            _activityRepository = activityRepository;
        }

        public async Task<Booking> CreateBookingAsync(int memberId, int activityId)
        {
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
    }
}