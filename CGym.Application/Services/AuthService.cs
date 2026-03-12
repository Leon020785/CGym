using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using System;
using System.Threading.Tasks;

    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RegisterUserAsync(string username, string email, string password)
        {
            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Create user entity
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            // Save user to database
            await _userRepository.AddUserAsync(user);
        }
    }
