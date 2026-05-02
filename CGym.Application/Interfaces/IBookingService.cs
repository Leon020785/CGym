using CGym.Domain.Entities;

namespace CGym.Application.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetBookingsAsync();
        Task<Booking> CreateBookingAsync(int memberId, int activityId);
        // Opretter en booking hvis der er plads på holdet

        Task DeleteBookingAsync(int id);
        Task<IEnumerable<Booking>> GetByMemberIdAsync(int memberId);
    }
}