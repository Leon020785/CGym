using System.Net.Http.Json;
using CGym.Frontend.Models;

namespace CGym.Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;

        public string? Token { get; private set; }

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
        }
    }
}