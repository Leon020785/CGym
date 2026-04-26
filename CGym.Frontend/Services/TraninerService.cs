namespace CGym.Frontend.Services
{
    public class TrainerService
    {
        private readonly HttpClient _http; 

        public TrainerService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<List<TrainerApiModel>> GetTrainersAsync()
        {
            return await _http.GetFromJsonAsync<List<TrainerApiModel>>("api/trainer") ?? new();
        }

        public async Task<TrainerApiModel?> CreateTrainerAsync(string name)
        {
            var response = await _http.PostAsJsonAsync("api/trainer", new
            {
                name

            });
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TrainerApiModel>();
        }
    }

    public class TrainerApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}
