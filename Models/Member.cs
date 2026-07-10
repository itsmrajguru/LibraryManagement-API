using LibraryManagementAPI.Models.Enums;

namespace LibraryManagementAPI.Models
{
    // A library member who can borrow books
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public MembershipType MembershipType { get; set; } = MembershipType.Standard;
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;

        // Navigation: all book issues for this member
        public List<Issue> Issues { get; set; } = new();
    }
}
