using CGym.Frontend.Models;

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

            return await _http.GetFromJsonAsync<List<Member>>("api/members");
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

        private void AddAuthHeader()
        {
            if (!string.IsNullOrEmpty(_auth.Token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _auth.Token);
            }
        }
    }
}