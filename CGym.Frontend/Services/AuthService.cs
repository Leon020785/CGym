using System.Net.Http.Json;
using CGym.Frontend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CGym.Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;

        public event Action? OnChange;

        public string? Token { get; private set; }
        public int? CurrentMemberId { get; private set; }
        public string? CurrentEmail { get; private set; }
        public string? CurrentRole { get; private set; }
        public bool IsAdmin { get; private set; }

        public AuthService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<bool> Login(string email, string password)
        {
            return await LoginCore("api/auth/Login", email, password, requireAdmin: false);
        }

        public async Task<bool> AdminLogin(string email, string password)
        {
            return await LoginCore("api/auth/admin-login", email, password, requireAdmin: true);
        }

        private async Task<bool> LoginCore(string endpoint, string email, string password, bool requireAdmin)
        {
            var response = await _http.PostAsJsonAsync(endpoint, new
            {
                email,
                password
            });

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            var token = result?.Token;
            var role = GetRoleFromToken(token);

            if (string.IsNullOrWhiteSpace(token))
                return false;

            if (requireAdmin && role != "Admin")
                return false;

            if (!requireAdmin && role == "Admin")
                return false;

            Token = token;
            CurrentMemberId = GetUserIdFromToken(Token);
            CurrentEmail = GetEmailFromToken(Token);
            CurrentRole = role;
            IsAdmin = CurrentRole == "Admin";

            OnChange?.Invoke();
            return true;
        }

        public async Task<bool> Register(string username, string email, string password)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", new
            {
                username,
                email,
                password
            });
            return response.IsSuccessStatusCode;
        }
        public void Logout()
        {
            Token = null;
            CurrentMemberId = null;
            CurrentEmail = null;
            CurrentRole = null;
            IsAdmin = false;
            OnChange?.Invoke();
        }

        private static int? GetUserIdFromToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler(); 
            var jwt = handler.ReadJwtToken(token);

            var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                  ?? jwt.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;

            return int.TryParse(idClaim, out var id) ? id : null;
        }

        private static string? GetEmailFromToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            return jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                ?? jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        }

        private static string? GetRoleFromToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            return jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
                ?? jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        }
    }
}
