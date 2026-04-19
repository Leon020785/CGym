using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using System.Collections.Generic;
using System.Text;

namespace CGym.Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityService(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<IEnumerable<Activity>> GetActivitiesAsync()
        {
            return await _activityRepository.GetAllAsync();
        }

        public async Task<Activity?> GetByIdAsync(int id)
        {
            return await _activityRepository.GetByIdAsync(id); 
        }

        public async Task<Activity> CreateAsync(Activity activity)
        {
            await _activityRepository.AddAsync(activity);
            return activity;
        }

        public Task<Activity?> UpdateAsync(int id, Activity activity)
        {
            return _activityRepository.UpdateAsync(id, activity);
        }


        public async Task DeleteAsync(int id)
        {
            await _activityRepository.DeleteAsync(id); 
        }
    }
}
