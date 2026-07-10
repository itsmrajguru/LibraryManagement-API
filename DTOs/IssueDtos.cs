using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOs
{
    // What the client sends to issue a book to a member
    public class CreateIssueDto
    {
        [Required(ErrorMessage = "BookId is required!")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "MemberId is required!")]
        public int MemberId { get; set; }

        // How many days the member gets to keep the book (default 14)
        public int DueDays { get; set; } = 14;
    }

    // What we send back after an issue or return
    public class IssueDto
    {
        public int Id { get; set; }
        public string BookTitle { get; set; } = "";
        public string MemberName { get; set; } = "";
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = "";
    }
}
