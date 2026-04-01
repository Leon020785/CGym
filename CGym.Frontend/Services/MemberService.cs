using System.Net.Http.Json;
using CGym.Frontend.Models;

namespace CGym.Frontend.Services
{
    public class MemberService
    {
        private readonly HttpClient _http;

        public MemberService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<List<Member>> GetMembersAsync()
        {
            return await _http.GetFromJsonAsync<List<Member>>("api/members");
        }
    }
}