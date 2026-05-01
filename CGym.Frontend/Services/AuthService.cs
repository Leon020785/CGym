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
        public bool IsAdmin { get; private set; }

        public AuthService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<bool> Login(string email, string password)
        {
            var response = await _http.PostAsJsonAsync("api/auth/Login", new
            {
                email,
                password
            });

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            Token = result?.Token;
            CurrentMemberId = GetUserIdFromToken(Token);
            CurrentEmail = GetEmailFromToken(Token);
            IsAdmin = GetIsAdminFromToken(Token);

            OnChange?.Invoke();
            return true;
        }

        public async Task<(bool Succeeded, string? ErrorMessage)> Register(string username, string email, string password)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", new
            {
                username,
                email,
                password
            });
            if (response.IsSuccessStatusCode)
                return (true, null);

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                return (false, "Emailen er allerede i brug.");

            return (false, "Noget gik galt. Prøv igen.");
        }
        public void Logout()
        {
            Token = null;
            CurrentMemberId = null;
            CurrentEmail = null;
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

        private static bool GetIsAdminFromToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
                ?? jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            return role == "Admin";
        }
    }
}
