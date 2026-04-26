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

        public async Task<Trainer?> UpdateTrainerAsync(int id, Trainer trainer)
        {
            var existing = await _trainerRepository.GetTrainerAsync(id);

            if (existing == null)
                return null;

            existing.Name = trainer.Name;

            await _trainerRepository.UpdateAsync(existing);

            return existing;
        }

        public async Task DeleteTrainerAsync(int id)
        {
            var existing = await _trainerRepository.GetTrainerAsync(id);

            if (existing == null)
                throw new KeyNotFoundException("Trainer not found");

            await _trainerRepository.DeleteAsync(id);
        }

    }
}
