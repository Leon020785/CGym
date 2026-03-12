using CGym.Domain.Entities;
using System.Threading.Tasks;

namespace CGym.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
    }
}