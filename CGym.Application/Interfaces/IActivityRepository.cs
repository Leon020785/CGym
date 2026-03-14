using System;
using System.Collections.Generic;
using System.Text;
using CGym.Domain.Entities;

namespace CGym.Application.Interfaces
{
    public interface IActivityRepository
    {
        Task<IEnumerable<Activity>> GetAllAsync(); // hent alle aktiviteter fra databsen
        Task<Activity?> GetByIdAsync(int id); // aktivitet basert på id.
        Task AddAsync(Activity activity); 
        Task DeleteAsync(int id);



    }
}
