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
    }

    public class TrainerApiModel
    {
        public int id { get; set; }
        public string name { get; set; } = "";
    }
}
