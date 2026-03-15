using CGym.Domain.Entities;

namespace CGym.Application.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(int memberId, int activityId);
        // Opretter en booking hvis der er plads på holdet
    }
}