using CGym.Domain.Entities;

namespace CGym.Application.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<Trainer>> GetTrainersAsync();
        Task CreateTrainerAsync(Trainer trainer);
        Task<Trainer?> UpdateTrainerAsync(int id, Trainer trainer);
        Task DeleteTrainerAsync(int id);

    }
}
