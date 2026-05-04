using CGym.Frontend.Models;
using System.Net.Http.Json;

namespace CGym.Frontend.Services
{
    public class AdminService
    {
        private readonly HttpClient _http;
        private readonly AuthService _auth;

        public AdminService(IHttpClientFactory factory, AuthService auth)
        {
            _http = factory.CreateClient("API");
            _auth = auth;
        }

        public async Task<AdminProfile> GetAdminAsync(int userId)
        {
            AddAuthHeader();
            var profile = await _http.GetFromJsonAsync<AdminProfile>($"api/admins/{userId}");
            return profile ?? throw new Exception("Kunne ikke hente admin profil");
        }

        public async Task UpdateAdminAsync(int userId, string firstName, string lastName, string phoneNumber, string email)
        {
            AddAuthHeader();
            var response = await _http.PutAsJsonAsync($"api/admins/{userId}", new { firstName, lastName, phoneNumber, email });
            if (!response.IsSuccessStatusCode)
                throw new Exception("Kunne ikke opdatere admin profil");
        }

        private void AddAuthHeader()
        {
            _http.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(_auth.Token))
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _auth.Token);
        }
    }
}
