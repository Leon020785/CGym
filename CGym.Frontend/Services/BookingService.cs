using System.Net.Http.Json;
namespace CGym.Frontend.Services
{
    public class BookingService
    {
        private readonly HttpClient _http;
        
        public BookingService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task CancelBookingAsync(int bookingId)
        {
            var response = await _http.DeleteAsync($"api/booking/{bookingId}");
            response.EnsureSuccessStatusCode(); 
        }

        public async Task<List<BookingApiModel>> GetBookingsAsync()
        {
            return await _http.GetFromJsonAsync<List<BookingApiModel>>("api/booking") ?? new();
        }

        // Opretter en booking i backend via POST endpoint.
        public async Task CreateBookingAsync(int memberId, int activityId)
        {
            // Kalder booking endpoint med memberId og activityId som query params.
            var response = await _http.PostAsync($"api/booking?memberId={memberId}&activityId={activityId}", null);

            // Kaster exception hvis statuskode ikke er success.
            response.EnsureSuccessStatusCode();
        }
    }

    public class BookingApiModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int ActivityId { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingActivityApiModel? Activity { get; set; }
        
    }

    public class BookingActivityApiModel
    {
        public string Name { get; set; } = "";
        public DateTime StartTime { get; set; }
        public BookingTrainerApiModel? Trainer { get; set; }
    }

    public class BookingTrainerApiModel
    {
        public string Name { get; set; } = "";
    }
}
