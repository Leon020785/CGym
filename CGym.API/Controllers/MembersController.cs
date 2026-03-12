using CGym.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


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

        // POST: api/members
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMemberRequest request)
        {
            var member = await _memberService.CreateMemberAsync(
                request.FirstName,
                request.LastName,
                request.Email);
            return CreatedAtAction(nameof(GetById), new { id = member.Id }, member);
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
}