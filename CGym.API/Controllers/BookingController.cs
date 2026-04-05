using CGym.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CGym.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetBookingsAsync();
            return Ok(bookings); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(int memberId, int activityId)
        {
            var booking = await _bookingService.CreateBookingAsync(memberId, activityId);

            return Ok(booking);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookingService.DeleteBookingAsync(id);
            return NoContent(); 
        }
    }
}