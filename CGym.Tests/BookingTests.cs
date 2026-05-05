using CGym.Domain.Entities;

namespace CGym.Tests
{
    public class BookingTests
    {
        [Fact]
        public void Booking_ShouldHaveCorrectMemberId()
        {
            var booking = new Booking
            {
                Id = 1,
                MemberId = 5,
                ActivityId = 3,
                BookingDate = DateTime.UtcNow
            };
            Assert.Equal(5, booking.MemberId);
        }

        [Fact]
        public void Booking_ShouldHaveCorrectActivityId()
        {
            var booking = new Booking
            {
                Id = 1,
                MemberId = 5,
                ActivityId = 3,
                BookingDate = DateTime.UtcNow
            };
            Assert.Equal(3, booking.ActivityId);
        }

        [Fact]
        public void Booking_DateShouldNotBeDefault()
        {
            var booking = new Booking
            {
                Id = 1,
                MemberId = 5,
                ActivityId = 3,
                BookingDate = DateTime.UtcNow
            };
            Assert.NotEqual(default(DateTime), booking.BookingDate);
        }
    }
}