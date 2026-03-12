using Microsoft.EntityFrameworkCore;
using CGym.Domain.Entities;

namespace CGym.Infrastructure.Persistence
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; } // vis member ikke findes så tjekker vi her 
        public DbSet<User> Users { get; set; }

    }
}