using CGym.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace CGym.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        // Henter alle trænere fra databasen

        Task<Booking?> GetByIdAsync(int id);
        // Henter én træner baseret på ID

        Task AddAsync(Booking booking);
        Task DeleteAsync(int id);

    }
}
