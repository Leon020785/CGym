
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

        public async Task<ActivityApiModel?> CreateActivityAsync(CreateActivityRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/activity", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ActivityApiModel>(); 
        }

        public async Task DeleteActivityAsync (int id)
        {
            var response = await _http.DeleteAsync($"api/activity/{id}");
            response.EnsureSuccessStatusCode();
        }
    }

    public class ActivityApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ActivityTrainerApiModel? Trainer { get; set; }
        public DateTime StartTime { get; set; }
        public int Capacity { get; set; }
    }
    public class ActivityTrainerApiModel
    {
        public string Name { get; set; } = "";
        public int Id { get; set; }
    }

    public class CreateActivityRequest
    {
        public string Name { get; set; } = "";
        public DateTime StartTime { get; set; }
        public int Capacity { get; set; }
        public int TrainerId { get; set; }
    }
}
