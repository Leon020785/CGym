using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IAdminRepository _adminRepository;
    private readonly IMemberRepository _memberRepository;

    public AuthService(IUserRepository userRepository, IEmailService emailService, IConfiguration configuration, IAdminRepository adminRepository, IMemberRepository memberRepository)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _configuration = configuration;
        _adminRepository = adminRepository;
        _memberRepository = memberRepository;
    }

    public async Task<User> RegisterUserAsync(string username, string email, string password, bool isAdmin = false)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email is already in use.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            IsAdmin = isAdmin
        };

        await _userRepository.AddUserAsync(user);
        return user;
    }

    public async Task<User?> LoginUserAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user == null)
            return null;

        var validPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        if (!validPassword)
            return null;

        return user;
    }

    public async Task<bool> ChangePasswordAsync(string email, string currentPassword, string newPassword)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
            return false;

        if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _userRepository.UpdateUserAsync(user);
        return true;
    }

    public async Task ForgotPasswordAsync(string email, string resetPath = "reset-password")
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null) return;

        var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var urlToken = WebUtility.UrlEncode(rawToken);

        user.PasswordResetToken = rawToken;
        user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);
        await _userRepository.UpdateUserAsync(user);

        var frontendUrl = _configuration["FrontendUrl"] ?? "https://localhost:7298";
        var resetLink = $"{frontendUrl}/{resetPath}?token={urlToken}";
        await _emailService.SendPasswordResetEmailAsync(user.Email, resetLink);
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        var user = await _userRepository.GetUserByResetTokenAsync(token);
        if (user == null) return false;
        if (user.PasswordResetTokenExpiry < DateTime.UtcNow) return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiry = null;
        await _userRepository.UpdateUserAsync(user);
        return true;
    }

    public async Task<string> GenerateJwtTokenAsync(User user, int? memberId = null)
    {
        var jwtKey = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException("Jwt:Key is not configured.");
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));


        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
        };

        if (user.IsAdmin)
        {
            var admin = await _adminRepository.GetByUserIdAsync(user.Id);
            if (admin != null)
            {
                claims.Add(new Claim("FirstName", admin.FirstName ?? ""));
                claims.Add(new Claim("LastName", admin.LastName ?? ""));
            }
        }
        else if (memberId.HasValue)
        {
            claims.Add(new Claim("MemberId", memberId.Value.ToString()));
            var member = await _memberRepository.GetByIdAsync(memberId.Value);
            if (member != null)
            {
                claims.Add(new Claim("FirstName", member.FirstName ?? ""));
                claims.Add(new Claim("LastName", member.LastName ?? ""));
            }
        }

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
