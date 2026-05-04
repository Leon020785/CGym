using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using CGym.Infrastructure.Persistence;

namespace CGym.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly GymDbContext _context;

        public AdminRepository(GymDbContext context)
        {
            _context = context;
        }

        public async Task<Admin> AddAsync(Admin admin)
        {
            await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();
            return admin;
        }
    }
}
