using CGym.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CGym.API.Controllers
{
    [ApiController]
    [Route("api/admins")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetById(int userId)
        {
            var admin = await _adminRepository.GetByUserIdAsync(userId);
            if (admin == null) return NotFound();
            return Ok(new { admin.FirstName, admin.LastName, admin.PhoneNumber, admin.Email });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{userId:int}")]
        public async Task<IActionResult> Update(int userId, [FromBody] UpdateAdminRequest request)
        {
            var admin = await _adminRepository.UpdateAsync(userId, request.FirstName, request.LastName, request.PhoneNumber, request.Email);
            if (admin == null) return NotFound();
            return Ok(new { admin.FirstName, admin.LastName, admin.PhoneNumber, admin.Email });
        }
    }

    public record UpdateAdminRequest(string FirstName, string LastName, string PhoneNumber, string Email);
}
