using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using CGym.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CGym.Infrastructure.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly GymDbContext _context;

        public ActivityRepository(GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Activity>> GetAllAsync()
        {
            return await _context.Activities
                .Include(a => a.Trainer)
                .ToListAsync();
        }

        public async Task<Activity?> GetByIdAsync(int id)
        {
            return await _context.Activities
                .Include(a => a.Trainer)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
        }

        public async Task<Activity?> UpdateAsync(int id, Activity activity)
        {
            var existing = await _context.Activities.FirstOrDefaultAsync(a => a.Id == id);

            if (existing == null)
                return null;

            existing.Name = activity.Name;
            existing.StartTime = activity.StartTime;
            existing.Capacity = activity.Capacity;
            existing.TrainerId = activity.TrainerId;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }


        public async Task DeleteAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);

            if (activity != null)
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }
    }
}