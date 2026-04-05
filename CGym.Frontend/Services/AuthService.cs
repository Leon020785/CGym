using System.Net.Http.Json;
using CGym.Frontend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CGym.Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;

        public string? Token { get; private set; }
        public int? CurrentMemberId { get; private set; }
        public string? CurrentEmail { get; private set; }

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
    }
}