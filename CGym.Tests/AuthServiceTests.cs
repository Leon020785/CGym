using BCrypt.Net;

namespace CGym.Tests
{
    public class AuthServiceTests
    {
        [Fact]
        public void Password_ShouldBeHashed()
        {
            var password = "MinAdgangskode123";
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            Assert.NotEqual(password, hash);
        }

        [Fact]
        public void Password_ShouldVerifyCorrectly()
        {
            var password = "MinAdgangskode123";
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            var result = BCrypt.Net.BCrypt.Verify(password, hash);
            Assert.True(result);
        }

        [Fact]
        public void WrongPassword_ShouldFailVerification()
        {
            var password = "MinAdgangskode123";
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            var result = BCrypt.Net.BCrypt.Verify("ForkertPassword", hash);
            Assert.False(result);
        }
    }
}