namespace CGym.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int ActivityId { get; set; }

        public DateTime BookingDate { get; set; }

        public Member Member { get; set; }

        public Activity Activity { get; set; }
    }
}