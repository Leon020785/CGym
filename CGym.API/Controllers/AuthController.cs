using Microsoft.AspNetCore.Mvc;
using CGym.Application.Services;
using CGym.API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace CGym.API.Controllers
{
    
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _authService.RegisterUserAsync(
                request.Username,
                request.Email,
                request.Password
            );

            return Ok("User registered successfully");
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

            var token = _authService.GenerateJwtToken(user);

            return Ok(new { token });

        }
    }
}