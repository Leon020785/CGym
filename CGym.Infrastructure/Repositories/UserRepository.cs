using CGym.Infrastructure.Persistence;
using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CGym.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GymDbContext _context;

        public UserRepository(GymDbContext context)
        {
            _context = context;
        }
        
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}