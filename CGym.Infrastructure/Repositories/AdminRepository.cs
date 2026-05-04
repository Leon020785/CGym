using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using CGym.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Admin?> GetByUserIdAsync(int userId)
        {
            return await _context.Admins
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<Admin?> UpdateAsync(int userId, string firstName, string lastName, string phoneNumber, string email)
        {
            var admin = await _context.Admins
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (admin == null) return null;

            admin.FirstName = firstName;
            admin.LastName = lastName;
            admin.PhoneNumber = phoneNumber;
            admin.Email = email;
            admin.User.Email = email;

            await _context.SaveChangesAsync();
            return admin;
        }
    }
}
