using CGym.Frontend.Models;

using System.Net.Http.Json;

namespace CGym.Frontend.Services
{
    public class MemberService
    {
        private readonly HttpClient _http;
        private readonly AuthService _auth;

        public MemberService(IHttpClientFactory factory, AuthService auth)
        {
            _http = factory.CreateClient("API");
            _auth = auth;
        }

        public async Task<List<Member>> GetMembersAsync()
        {
            AddAuthHeader();

            return await _http.GetFromJsonAsync<List<Member>>("api/members") ?? [];
        }

        public async Task<Member> GetMemberAsync(int id)
        {
            AddAuthHeader();

            var member = await _http.GetFromJsonAsync<Member>($"api/members/{id}");
            return member ?? throw new Exception("Kunne ikke hente medlem");
        }

        public async Task CreateMemberAsync(string firstName, string lastName, string email)
        {
            AddAuthHeader();

            var request = new
            {
                firstName,
                lastName,
                email
            };

            var response = await _http.PostAsJsonAsync("api/members", request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Kunne ikke oprette member");
            }
        }

        public async Task UpdateMemberAsync(int id, string firstName, string lastName, string phoneNumber)
        {
            AddAuthHeader();

            var request = new
            {
                firstName,
                lastName,
                phoneNumber
            };

            var response = await _http.PutAsJsonAsync($"api/members/{id}", request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Kunne ikke opdatere medlem");
            }
        }

        public async Task UpdateEmailAsync(int id, string newEmail)
        {
            AddAuthHeader();

            var request = new { newEmail };
            var response = await _http.PutAsJsonAsync($"api/members/{id}/email", request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Kunne ikke opdatere email");
            }
        }

        private void AddAuthHeader()
        {
            _http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(_auth.Token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _auth.Token);
            }
        }
    }
}
