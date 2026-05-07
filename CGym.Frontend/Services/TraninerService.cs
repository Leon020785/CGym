namespace CGym.Frontend.Services
{
    public class TrainerService
    {
        private readonly HttpClient _http;
        private readonly AuthService _auth;

        public TrainerService(IHttpClientFactory factory, AuthService auth)
        {
            _http = factory.CreateClient("API");
            _auth = auth;
        }

        public async Task<List<TrainerApiModel>> GetTrainersAsync()
        {
            return await _http.GetFromJsonAsync<List<TrainerApiModel>>("api/trainer") ?? new();
        }

        public async Task<TrainerApiModel?> CreateTrainerAsync(
            string name,
            string email,
            string phoneNumber,
            string availability)
        {
            AddAuthHeader();
            var response = await _http.PostAsJsonAsync("api/trainer", new
            {
                name,
                email,
                phoneNumber,
                availability
            });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TrainerApiModel>();
        }

        public async Task<TrainerApiModel?> UpdateTrainerAsync(
            int id,
            string name,
            string email,
            string phoneNumber,
            string availability)
        {
            AddAuthHeader();
            var response = await _http.PutAsJsonAsync($"api/trainer/{id}", new
            {
                name,
                email,
                phoneNumber,
                availability
            });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TrainerApiModel>();
        }

        public async Task DeleteTrainerAsync(int id)
        {
            AddAuthHeader();
            var response = await _http.DeleteAsync($"api/trainer/{id}");
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

    public class TrainerApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Availability { get; set; } = "";

    }
}
