using CGym.Domain.Entities;

namespace CGym.Tests
{
    public class ActivityTests
    {
        [Fact]
        public void Activity_ShouldHaveCorrectCapacity()
        {
            var activity = new Activity
            {
                Id = 1,
                Name = "HIIT Training",
                Capacity = 20,
                StartTime = DateTime.UtcNow
            };
            Assert.Equal(20, activity.Capacity);
        }

        [Fact]
        public void Activity_NameShouldNotBeEmpty()
        {
            var activity = new Activity
            {
                Id = 1,
                Name = "Yoga",
                Capacity = 15,
                StartTime = DateTime.UtcNow
            };
            Assert.NotEmpty(activity.Name);
        }

        [Fact]
        public void Activity_CapacityShouldBePositive()
        {
            var activity = new Activity
            {
                Id = 1,
                Name = "Spinning",
                Capacity = 10,
                StartTime = DateTime.UtcNow
            };
            Assert.True(activity.Capacity > 0);
        }

        [Fact]
        public void Activity_StartTimeShouldNotBeDefault()
        {
            var activity = new Activity
            {
                Id = 1,
                Name = "Yoga",
                Capacity = 15,
                StartTime = DateTime.UtcNow
            };
            Assert.NotEqual(default(DateTime), activity.StartTime);
        }
    }
}