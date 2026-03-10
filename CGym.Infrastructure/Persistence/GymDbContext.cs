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

        public DbSet<Member> Members { get; set; }
    }
}