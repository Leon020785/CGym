using System.Collections.Generic;

namespace CGym.Domain.Entities
{
    public class Trainer
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Availability { get; set; }

        public ICollection<Activity> Activities { get; set; }
        // En træner kan have mange aktiviteter
    }
}