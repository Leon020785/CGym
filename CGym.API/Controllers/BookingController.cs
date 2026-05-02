using CGym.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> CreateBooking([FromQuery] int memberId, [FromQuery] int activityId)
        {
            try
            {
                var booking = await _bookingService.CreateBookingAsync(memberId, activityId);
                return Ok(booking);
            }
            catch (InvalidOperationException ex)
            {
                var danskBesked = ex.Message switch
                {
                    "Member has already booked this activity." => "Du er allerede tilmeldt dette hold.",
                    "Activity is full." => "Dette hold er desværre fuldt.",
                    _ => ex.Message
                };
                return BadRequest(new { detail = danskBesked });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { detail = "Aktivitet eller medlem blev ikke fundet." });
            }
        }

        [Authorize]
        [HttpGet("member/{memberId:int}")]
        public async Task<IActionResult> GetByMember(int memberId)
        {
            var isAdmin = User.FindFirstValue(ClaimTypes.Role) == "Admin";
            var memberIdClaim = User.FindFirstValue("MemberId");

            if (!isAdmin && (memberIdClaim == null || memberIdClaim != memberId.ToString()))
                return Forbid();

            var bookings = await _bookingService.GetByMemberIdAsync(memberId);
            return Ok(bookings);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookingService.DeleteBookingAsync(id);
            return NoContent(); 
        }
    }
}