using System.Net.Http.Json;

namespace CGym.Frontend.Services
{
    public class ActivityService
    {
        private readonly HttpClient _http;

        public ActivityService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<List<ActivityApiModel>> GetActivitiesAsync()
        {
            return await _http.GetFromJsonAsync<List<ActivityApiModel>>("api/activity") ?? new();
        }
    }

    public class ActivityApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime StartTime { get; set; }
        public int Capacity { get; set; }
    }
}
