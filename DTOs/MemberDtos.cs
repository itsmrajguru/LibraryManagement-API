using System.ComponentModel.DataAnnotations;
using LibraryManagementAPI.Models.Enums;

namespace LibraryManagementAPI.DTOs
{
    // What the client sends when registering a new member
    public class CreateMemberDto
    {
        [Required(ErrorMessage = "Name is required!")]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = "";

        [Phone(ErrorMessage = "Invalid phone number.")]
        [MaxLength(15)]
        public string Phone { get; set; } = "";

        public MembershipType MembershipType { get; set; } = MembershipType.Standard;
    }

    // What we send back when returning member info
    public class MemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string MembershipType { get; set; } = "";
        public DateTime JoinDate { get; set; }
    }
}
