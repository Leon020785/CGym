using System;
using System.Collections.Generic;
using System.Text;

namespace CGym.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; } // EF Core kan sætte værdien, andre klasser kan ikke ændre den.
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;// vi bruger Hash for sikkerhed her.

        public DateTime CreatedAt { get; set;}
        public bool IsAdmin { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
    }
}
