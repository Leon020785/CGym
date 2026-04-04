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

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromQuery] int memberId, [FromQuery] int activityId)
        {
            try
            {
                var booking = await _bookingService.CreateBookingAsync(memberId, activityId);
                return Ok(booking); // eller Created(...) senere
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message }); // 400
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // 404
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // 409
            }
        }
    }
}