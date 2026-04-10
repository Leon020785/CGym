using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CGym.Frontend.Services
{
    public class BookingService
    {
        private readonly HttpClient _http;
        private readonly AuthService _auth;

        public BookingService(IHttpClientFactory factory, AuthService auth)
        {
            _http = factory.CreateClient("API");
            _auth = auth;
        }

        public async Task CancelBookingAsync(int bookingId)
        {
            AddAuthHeader();
            var response = await _http.DeleteAsync($"api/booking/{bookingId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<BookingApiModel>> GetBookingsAsync()
        {
            AddAuthHeader();
            return await _http.GetFromJsonAsync<List<BookingApiModel>>("api/booking") ?? new();
        }

        // Opretter en booking i backend via POST endpoint.
        public async Task CreateBookingAsync(int memberId, int activityId)
        {
            AddAuthHeader();

            // Kalder booking endpoint med memberId og activityId som query params.
            var response = await _http.PostAsync($"api/booking?memberId={memberId}&activityId={activityId}", null);

            if (response.IsSuccessStatusCode)
                return;

            var problem = await response.Content.ReadFromJsonAsync<ApiProblemDetails>();

            if (!string.IsNullOrWhiteSpace(problem?.Detail))
                throw new InvalidOperationException(problem.Detail);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new InvalidOperationException("Du skal logge ind igen for at booke.");

            // Kaster exception hvis statuskode ikke er success.
            response.EnsureSuccessStatusCode();
        }

        private void AddAuthHeader()
        {
            if (!string.IsNullOrEmpty(_auth.Token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _auth.Token);
            }
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

    public class ApiProblemDetails
    {
        public string? Detail { get; set; }
    }
}
