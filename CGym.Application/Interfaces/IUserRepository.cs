using CGym.Domain.Entities;
using System.Threading.Tasks;

namespace CGym.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByResetTokenAsync(string token);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}