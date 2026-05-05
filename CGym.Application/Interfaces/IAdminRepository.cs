using CGym.Domain.Entities;

namespace CGym.Application.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin> AddAsync(Admin admin);
        Task<Admin?> GetByUserIdAsync(int userId);
        Task<Admin?> UpdateAsync(int userId, string firstName, string lastName, string phoneNumber, string email);
    }
}
