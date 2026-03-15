using System;

namespace CGym.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        // en booking tilføje en member 

        public int ActivityId { get; set; }

        public DateTime BookingDate { get; set; }

        public Member Member { get; set; }
        // en member kan have mange bookings

        public Activity Activity { get; set; }
    }
}