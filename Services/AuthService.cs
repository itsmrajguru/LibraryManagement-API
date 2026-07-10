using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Helpers;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Services
{
    // Handles user registration and login with JWT token generation
    public class AuthService : IAuthService
    {
        private readonly LibraryDbContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(LibraryDbContext context, JwtHelper jwtHelper, ILogger<AuthService> logger)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _logger = logger;
        }

        public AuthResponseDto Register(RegisterDto dto)
        {
            // Check if email is already taken
            if (_context.Users.Any(u => u.Email == dto.Email))
                throw new InvalidOperationException("Email is already registered!");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), // hash before storing
                Role = dto.Role
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            _logger.LogInformation("New user registered: {Email}", user.Email);

            var token = _jwtHelper.GenerateToken(user);
            return new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Role = user.Role.ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        public AuthResponseDto Login(LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            // Verify user exists and password matches the stored hash
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password!");

            _logger.LogInformation("User logged in: {Email}", user.Email);

            var token = _jwtHelper.GenerateToken(user);
            return new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Role = user.Role.ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }
    }
}
