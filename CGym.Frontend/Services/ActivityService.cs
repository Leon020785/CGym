
using System.Net.Http.Json;

namespace CGym.Frontend.Services
{
    public class ActivityService
    {
        private readonly HttpClient _http;
        private readonly AuthService _auth;

        public ActivityService(IHttpClientFactory factory, AuthService auth)
        {
            _http = factory.CreateClient("API");
            _auth = auth;
        }

        public async Task<List<ActivityApiModel>> GetActivitiesAsync()
        {
            return await _http.GetFromJsonAsync<List<ActivityApiModel>>("api/activity") ?? new();
        }

        public async Task<ActivityApiModel?> CreateActivityAsync(CreateActivityRequest request)
        {
            AddAuthHeader();
            var response = await _http.PostAsJsonAsync("api/activity", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ActivityApiModel>();
        }

        public async Task<ActivityApiModel?> UpdateActivityAsync(int id, CreateActivityRequest request)
        {
            AddAuthHeader();
            var response = await _http.PutAsJsonAsync($"api/activity/{id}", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ActivityApiModel>();
        }

        public async Task DeleteActivityAsync(int id)
        {
            AddAuthHeader();
            var response = await _http.DeleteAsync($"api/activity/{id}");
            response.EnsureSuccessStatusCode();
        }

        private void AddAuthHeader()
        {
            _http.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(_auth.Token))
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _auth.Token);
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
        public int? TrainerId { get; set; }
    }
}
