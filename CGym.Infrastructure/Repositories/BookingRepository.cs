using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using CGym.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CGym.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly GymDbContext _context;

        public BookingRepository(GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.Member)
                .Include(b => b.Activity)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Member)
                .Include(b => b.Activity)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }
    }
}