using System;
using System.Collections.Generic;
using System.Text;
using CGym.Domain.Entities;

namespace CGym.Application.Interfaces
{
    public interface ITrainerRepository
    {
        Task<IEnumerable<Trainer>> GetTrainersAsync();
        // Henter alle trænere fra databasen

        Task<Trainer?> GetTrainerAsync();
        // Henter én træner baseret på ID

        Task AddAsync(Trainer trainer);
        Task UpdateAsync(Trainer trainer);
    }
}
