using CGym.Application.Interfaces;
using CGym.Domain.Entities;

namespace CGym.Application.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly ITrainerRepository _trainerRepository;

        public TrainerService(ITrainerRepository trainerRepository)
        {
            _trainerRepository = trainerRepository;
        }

        public async Task<IEnumerable<Trainer>> GetTrainersAsync()
        {
            return await _trainerRepository.GetTrainersAsync();
        }
        public async Task CreateTrainerAsync(Trainer trainer)
        {
            await _trainerRepository.AddAsync(trainer);

        }
    }
}
