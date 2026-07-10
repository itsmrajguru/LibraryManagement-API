namespace LibraryManagementAPI.DTOs
{
    // What we send back for a fine record
    public class FineDto
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public string BookTitle { get; set; } = "";
        public string MemberName { get; set; } = "";
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidDate { get; set; }
    }
}
