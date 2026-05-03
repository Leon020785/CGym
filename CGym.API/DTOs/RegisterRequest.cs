namespace CGym.API.DTOs
{
    public class RegisterRequest
    {
        public string Username {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
