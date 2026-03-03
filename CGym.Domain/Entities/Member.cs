using System;
using System.Collections.Generic;
using System.Text;

namespace CGym.Domain.Entities
{
    public class Member
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Member(string firstName, string lastName, string email)
        {
            Id = 0;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CreatedAt = DateTime.UtcNow;
            
        }
    }
}
