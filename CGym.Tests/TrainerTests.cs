using CGym.Domain.Entities;

namespace CGym.Tests
{
    public class TrainerTests
    {
        [Fact]
        public void Trainer_NameShouldNotBeEmpty()
        {
            var trainer = new Trainer
            {
                Id = 1,
                Name = "Sarah Johnson"
            };
            Assert.NotEmpty(trainer.Name);
        }

        [Fact]
        public void Trainer_ShouldHaveCorrectId()
        {
            var trainer = new Trainer
            {
                Id = 3,
                Name = "John Doe"
            };
            Assert.Equal(3, trainer.Id);
        }
    }
}