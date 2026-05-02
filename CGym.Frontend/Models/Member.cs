namespace CGym.Frontend.Models
{
    public class Member
    {
        public int Id { get; set; } // id fra db
        public string FirstName { get; set; } = "" ; 
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";

    }
}
