using System.Net.Http.Json;
using CGym.Frontend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CGym.Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;

        public event Action? OnChange;

        public string? Token { get; private set; }
        public int? CurrentUserId { get; private set; }
        public int? CurrentMemberId { get; private set; }
        public string? CurrentEmail { get; private set; }
        public string? CurrentUsername { get; private set; }
        public string? CurrentFirstName { get; private set; }
        public string? CurrentLastName { get; private set; }
        public string? CurrentRole { get; private set; }
        public bool IsAdmin { get; private set; }
        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

        public AuthService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        private const string InvalidLoginMessage = "Forkert email eller adgangskode.";
        private const string AdminMustUseAdminLoginMessage = "Du er admin. Brug admin-login i stedet.";
        private const string UserHasNoAdminAccessMessage = "Du har ikke adgang. Kun admins kan logge ind her.";

        public async Task<(bool Succeeded, string? ErrorMessage)> Login(string email, string password)
        {
            return await LoginCore("api/auth/Login", email, password, requireAdmin: false);
        }

        public async Task<(bool Succeeded, string? ErrorMessage)> AdminLogin(string email, string password)
        {
            return await LoginCore("api/auth/admin-login", email, password, requireAdmin: true);
        }

        private async Task<(bool Succeeded, string? ErrorMessage)> LoginCore(string endpoint, string email, string password, bool requireAdmin)
        {
            var response = await _http.PostAsJsonAsync(endpoint, new
            {
                email,
                password
            });

            if (!response.IsSuccessStatusCode)
            {
                var backendMessage = await response.Content.ReadAsStringAsync();
                return (false, MapLoginError(backendMessage, requireAdmin));
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            var token = result?.Token;
            var role = GetRoleFromToken(token);

            if (string.IsNullOrWhiteSpace(token))
                return (false, InvalidLoginMessage);

            if (requireAdmin && role != "Admin")
                return (false, UserHasNoAdminAccessMessage);

            if (!requireAdmin && role == "Admin")
                return (false, AdminMustUseAdminLoginMessage);

            Token = token;
            CurrentUserId = GetUserIdFromToken(Token);
            CurrentMemberId = GetMemberIdFromToken(Token);
            CurrentEmail = GetEmailFromToken(Token);
            CurrentUsername = GetUsernameFromToken(Token);
            CurrentFirstName = GetFirstNameFromToken(Token);
            CurrentLastName = GetLastNameFromToken(Token);
            CurrentRole = role;
            IsAdmin = CurrentRole == "Admin";

            OnChange?.Invoke();
            return (true, null);
        }

        public async Task<(bool Succeeded, string? ErrorMessage)> Register(string username, string email, string password)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", new
            {
                username,
                email,
                password
            });
            if (response.IsSuccessStatusCode)
                return (true, null);

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                return (false, "Emailen er allerede i brug.");

            return (false, "Noget gik galt. Prøv igen.");
        }
        public async Task<(bool Succeeded, string? ErrorMessage)> ForgotPasswordAsync(string email)
        {
            var response = await _http.PostAsJsonAsync("api/auth/forgot-password", new { email });
            if (response.IsSuccessStatusCode)
                return (true, null);

            return (false, "Noget gik galt. Prøv igen.");
        }

        public async Task<(bool Succeeded, string? ErrorMessage)> ResetPasswordAsync(string token, string newPassword)
        {
            var response = await _http.PostAsJsonAsync("api/auth/reset-password", new { token, newPassword });
            if (response.IsSuccessStatusCode)
                return (true, null);

            var msg = await response.Content.ReadAsStringAsync();
            return (false, msg.Trim('"'));
        }

        public void UpdateCurrentEmail(string newEmail)
        {
            CurrentEmail = newEmail;
            OnChange?.Invoke();
        }

        public void Logout()
        {
            Token = null;
            CurrentUserId = null;
            CurrentMemberId = null;
            CurrentEmail = null;
            CurrentUsername = null;
            CurrentFirstName = null;
            CurrentLastName = null;
            CurrentRole = null;
            IsAdmin = false;
            OnChange?.Invoke();
        }

        private static string MapLoginError(string? backendMessage, bool requireAdmin)
        {
            var normalizedMessage = NormalizeErrorMessage(backendMessage);

            if (!requireAdmin && normalizedMessage == "Admin users must use the admin login endpoint")
                return AdminMustUseAdminLoginMessage;

            if (requireAdmin && normalizedMessage == "Only admin users can log in through this endpoint")
                return UserHasNoAdminAccessMessage;

            return InvalidLoginMessage;
        }

        private static string NormalizeErrorMessage(string? message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return "";

            return message.Trim().Trim('"').TrimEnd('.');
        }

        private static int? GetUserIdFromToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwt.Claims);
            var principal = new ClaimsPrincipal(identity);

            var nameIdClaim = principal.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier ||
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier" ||
                c.Type == "nameid" ||
                c.Type == "sub");

            return nameIdClaim != null && int.TryParse(nameIdClaim.Value, out var uid) ? uid : (int?)null;
        }

        private static int? GetMemberIdFromToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler(); 
            var jwt = handler.ReadJwtToken(token);

            var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == "MemberId")?.Value;

            return int.TryParse(idClaim, out var id) ? id : null;
        }

        private static string? GetEmailFromToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            return jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                ?? jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        }

        private static string? GetUsernameFromToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            return jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                ?? jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
        }

        private static string? GetFirstNameFromToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            return jwt.Claims.FirstOrDefault(c => c.Type == "FirstName")?.Value;
        }

        private static string? GetLastNameFromToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            return jwt.Claims.FirstOrDefault(c => c.Type == "LastName")?.Value;
        }

        private static string? GetRoleFromToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            return jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
                ?? jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        }
    }
}
