using LibraryManagementAPI.Models.Enums;

namespace LibraryManagementAPI.Models
{
    // A user who can log in to the system (admin, librarian, or member)
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = ""; // never store plain passwords
        public UserRole Role { get; set; } = UserRole.Member;
    }
}
