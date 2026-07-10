using System.ComponentModel.DataAnnotations;
using LibraryManagementAPI.Models.Enums;

namespace LibraryManagementAPI.DTOs
{
    // What the client sends to register a new user
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required!")]
        [MaxLength(50)]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required!")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = "";

        // Default role is Member, Admin can change this
        public UserRole Role { get; set; } = UserRole.Member;
    }

    // What the client sends to log in
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; } = "";
    }

    // What we send back after successful login
    public class AuthResponseDto
    {
        public string Token { get; set; } = "";
        public string Username { get; set; } = "";
        public string Role { get; set; } = "";
        public DateTime ExpiresAt { get; set; }
    }
}
