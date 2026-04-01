using System.Net.Http.Json;

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
            var response = await _http.PostAsJsonAsync("api/auth/login", new
            {
                email = email,
                password = password
            });

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            Token = result?.Token;

            return true;
        }

        private class LoginResponse
        {
            public string Token { get; set; }
        }
    }
}