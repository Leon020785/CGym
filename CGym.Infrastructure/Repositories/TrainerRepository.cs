using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using CGym.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CGym.Infrastructure.Repositories
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly GymDbContext _context;

        public TrainerRepository(GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trainer>> GetTrainersAsync()
        {
            return await _context.Trainers.ToListAsync();
        }

        public async Task<Trainer?> GetTrainerAsync(int id)
        {
            return await _context.Trainers.FindAsync(id);
        }

        public async Task AddAsync(Trainer trainer)
        {
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Trainer trainer)
        {
            _context.Trainers.Update(trainer);
            await _context.SaveChangesAsync();
        }
    }
}