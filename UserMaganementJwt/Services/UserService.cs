using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMaganementJwt.Interfaces;
using UserMaganementJwt.Models;

namespace UserMaganementJwt.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }


        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return _jwtService.GenerateJwtToken(user);
        }

        public async Task<User> RegisterAsync(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Username, password, and email are required.");
            }

            var existingEmail = await _userRepository.GetByEmailAsync(email);
            if (existingEmail != null)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Email = email
            };

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            return await _jwtService.ValidateTokenAsync(token);
        }


    }
}
