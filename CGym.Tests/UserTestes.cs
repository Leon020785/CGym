using CGym.Domain.Entities;

namespace CGym.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_IsAdminShouldBeFalseByDefault()
        {
            var user = new User
            {
                Email = "test@cgym.dk",
                PasswordHash = "hashedpassword",
                Username = "testuser"
            };
            Assert.False(user.IsAdmin);
        }

        [Fact]
        public void User_EmailShouldNotBeEmpty()
        {
            var user = new User
            {
                Email = "test@cgym.dk",
                PasswordHash = "hashedpassword",
                Username = "testuser"
            };
            Assert.NotEmpty(user.Email);
        }

        [Fact]
        public void User_AdminShouldBeTrue_WhenSetToTrue()
        {
            var user = new User
            {
                Email = "admin@cgym.dk",
                PasswordHash = "hashedpassword",
                Username = "admin",
                IsAdmin = true
            };
            Assert.True(user.IsAdmin);
        }
    }
}