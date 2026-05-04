using CGym.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace CGym.API.Controllers
{
    [Authorize]
    [ApiController]     // Håndterer alle HTTP-kald relateret til medlemmer

    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        // Modtager IMemberService via Dependency Injection
        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // GET: api/members
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var members = await _memberService.GetAllMembersAsync();
            return Ok(members);
        }

        // GET: api/members/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null) return NotFound();
            return Ok(member);
        }

        // PUT: api/members/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMemberRequest request)
        {
            var isAdmin = User.FindFirstValue(ClaimTypes.Role) == "Admin";
            var memberIdClaim = User.FindFirstValue("MemberId");

            if (!isAdmin && (memberIdClaim == null || memberIdClaim != id.ToString()))
                return Forbid();

            var member = await _memberService.UpdateAsync(id, request.FirstName, request.LastName, request.PhoneNumber);
            if (member == null) return NotFound();
            return Ok(member);
        }

        // PUT: api/members/5/email
        [HttpPut("{id}/email")]
        public async Task<IActionResult> UpdateEmail(int id, [FromBody] UpdateEmailRequest request)
        {
            var isAdmin = User.FindFirstValue(ClaimTypes.Role) == "Admin";
            var memberIdClaim = User.FindFirstValue("MemberId");

            if (!isAdmin && (memberIdClaim == null || memberIdClaim != id.ToString()))
                return Forbid();

            var member = await _memberService.UpdateEmailAsync(id, request.NewEmail);
            if (member == null) return NotFound();
            return Ok(member);
        }

        // DELETE: api/members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _memberService.DeleteAsync(id);
            return NoContent();
        }
    }

    // DTO — definerer hvad API'et modtager ved oprettelse
    public record CreateMemberRequest(string FirstName, string LastName, string Email);
    public record UpdateMemberRequest(string FirstName, string LastName, string PhoneNumber);
    public record UpdateEmailRequest(string NewEmail);
}
