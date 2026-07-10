using LibraryManagementAPI.Models.Enums;

namespace LibraryManagementAPI.Models
{
    // Tracks a book being borrowed by a member
    public class Issue
    {
        public int Id { get; set; }

        // Which book was issued
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        // Which member borrowed it
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; } = null; // null means still with member

        public IssueStatus Status { get; set; } = IssueStatus.Issued;

        // Navigation: fine attached to this issue (if overdue)
        public Fine? Fine { get; set; }
    }
}
