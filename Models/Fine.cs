namespace LibraryManagementAPI.Models
{
    // Fine charged when a book is returned late
    public class Fine
    {
        public int Id { get; set; }

        // Which issue caused this fine
        public int IssueId { get; set; }
        public Issue Issue { get; set; } = null!;

        public decimal Amount { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime? PaidDate { get; set; } = null; // null means not paid yet
    }
}
