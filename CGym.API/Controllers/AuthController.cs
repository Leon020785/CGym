using Microsoft.AspNetCore.Mvc;
using CGym.Application.Services;
using CGym.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using System.Security.Claims;

namespace CGym.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IMemberService _memberService;
        private readonly IAdminRepository _adminRepository;

        public AuthController(AuthService authService, IMemberService memberService, IAdminRepository adminRepository)
        {
            _authService = authService;
            _memberService = memberService;
            _adminRepository = adminRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _authService.RegisterUserAsync(
                request.Username,
                request.Email,
                request.Password
            );
            await _memberService.CreateMemberAsync(
                request.Username,
                "",
                request.Email,
                user.Id
            );

            return Ok("User registered successfully");
        }

        [HttpPost("admin/register")]
        public async Task<IActionResult> AdminRegister([FromBody] RegisterRequest request)
        {
            var user = await _authService.RegisterUserAsync(
                request.Username,
                request.Email,
                request.Password,
                isAdmin: true
            );

            var admin = new Admin
            {
                UserId = user.Id,
                Name = user.Username,
                Email = user.Email,
                CreatedAt = DateTime.UtcNow
            };
            await _adminRepository.AddAsync(admin);

            return Ok("Admin registered successfully");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.LoginUserAsync(
                request.Email,
                request.Password
                );

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            if (user.IsAdmin)
            {
                return Unauthorized("Admin users must use the admin login endpoint");
            }

            var memberId = await GetOrCreateMemberIdAsync(user.Username, user.Email, user.Id);
            var token = _authService.GenerateJwtToken(user, memberId);

            return Ok(new { token });
        }

        [HttpPost("admin-login")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginRequest request)
        {
            var user = await _authService.LoginUserAsync(
                request.Email,
                request.Password
                );

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            if (!user.IsAdmin)
            {
                return Unauthorized("Only admin users can log in through this endpoint");
            }

            var member = await _memberService.GetByEmailAsync(user.Email);
            var token = _authService.GenerateJwtToken(user, member?.Id);

            return Ok(new { token });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return Unauthorized();

            var success = await _authService.ChangePasswordAsync(email, request.CurrentPassword, request.NewPassword);
            if (!success)
                return BadRequest("Nuværende adgangskode er forkert.");

            return Ok("Adgangskode ændret.");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _authService.ForgotPasswordAsync(request.Email);
            return Ok("Hvis emailen findes, er et nulstillingslink sendt.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var success = await _authService.ResetPasswordAsync(request.Token, request.NewPassword);
            if (!success)
                return BadRequest("Ugyldigt eller udløbet nulstillingslink.");

            return Ok("Adgangskode nulstillet.");
        }

        private async Task<int> GetOrCreateMemberIdAsync(string username, string email, int userId)
        {
            var member = await _memberService.GetByEmailAsync(email);

            if (member == null)
            {
                member = await _memberService.CreateMemberAsync(username, "", email, userId);
            }

            return member.Id;
        }
    }
}
