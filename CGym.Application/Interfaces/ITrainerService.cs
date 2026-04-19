using CGym.Domain.Entities;

namespace CGym.Application.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<Trainer>> GetTrainersAsync(); 
    }
}
