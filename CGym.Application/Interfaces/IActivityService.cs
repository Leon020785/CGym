using CGym.Domain.Entities;
using System.Collections.Generic;

namespace CGym.Application.Interfaces
{
    public interface IActivityService
    {
        Task<IEnumerable<Activity>> GetActivitiesAsync();
        Task<Activity?> GetByIdAsync(int id);
        Task<Activity> CreateAsync(Activity activity);
        Task DeleteAsync(int id);  
    }
}
