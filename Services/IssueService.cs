using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services
{
    // Handles issuing and returning books
    public class IssueService : IIssueService
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<IssueService> _logger;

        // Fine rate: 5 rupees per day after due date
        private const decimal FinePerDay = 5m;

        public IssueService(LibraryDbContext context, ILogger<IssueService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IssueDto IssueBook(CreateIssueDto dto)
        {
            var book = _context.Books.Find(dto.BookId);
            if (book == null) throw new KeyNotFoundException($"Book with ID {dto.BookId} not found!");
            if (book.AvailableCopies <= 0) throw new InvalidOperationException("No copies available for this book!");

            var member = _context.Members.Find(dto.MemberId);
            if (member == null) throw new KeyNotFoundException($"Member with ID {dto.MemberId} not found!");

            // Block issue if member has unpaid fines above 50 rupees
            var unpaidFines = _context.Fines
                .Include(f => f.Issue)
                .Where(f => f.Issue.MemberId == dto.MemberId && !f.IsPaid)
                .Sum(f => (decimal?)f.Amount) ?? 0;

            if (unpaidFines > 50)
                throw new InvalidOperationException($"Member has unpaid fines of ₹{unpaidFines}. Please clear fines first.");

            var issue = new Issue
            {
                BookId = dto.BookId,
                MemberId = dto.MemberId,
                IssueDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(dto.DueDays),
                Status = IssueStatus.Issued
            };

            // Decrease available copies after issuing
            book.AvailableCopies--;

            _context.Issues.Add(issue);
            _context.SaveChanges();
            _logger.LogInformation("Book '{Title}' issued to member {MemberId}", book.Title, dto.MemberId);

            return MapToDto(issue, book.Title, member.Name);
        }

        public (bool success, string message, IssueDto? issue) ReturnBook(int issueId)
        {
            var issue = _context.Issues
                .Include(i => i.Book)
                .Include(i => i.Member)
                .FirstOrDefault(i => i.Id == issueId);

            if (issue == null) return (false, "Issue record not found!", null);
            if (issue.Status == IssueStatus.Returned) return (false, "Book already returned!", null);

            issue.ReturnDate = DateTime.UtcNow;
            issue.Status = IssueStatus.Returned;

            // Increase available copies when returned
            issue.Book.AvailableCopies++;

            // Auto-calculate fine if returned late
            if (issue.ReturnDate > issue.DueDate)
            {
                var daysLate = (issue.ReturnDate.Value - issue.DueDate).Days;
                var fineAmount = daysLate * FinePerDay;

                var fine = new Fine { IssueId = issue.Id, Amount = fineAmount };
                _context.Fines.Add(fine);
                _logger.LogInformation("Fine of ₹{Amount} created for issue {IssueId}", fineAmount, issueId);
            }

            _context.SaveChanges();
            _logger.LogInformation("Book '{Title}' returned by member {MemberId}", issue.Book.Title, issue.MemberId);

            return (true, "Book returned successfully!", MapToDto(issue, issue.Book.Title, issue.Member.Name));
        }

        public List<IssueDto> GetOverdueIssues()
        {
            var now = DateTime.UtcNow;
            return _context.Issues
                .Include(i => i.Book)
                .Include(i => i.Member)
                .Where(i => i.Status == IssueStatus.Issued && i.DueDate < now)
                .Select(i => MapToDto(i, i.Book.Title, i.Member.Name))
                .ToList();
        }

        public List<IssueDto> GetIssuesByMember(int memberId)
        {
            return _context.Issues
                .Include(i => i.Book)
                .Include(i => i.Member)
                .Where(i => i.MemberId == memberId)
                .Select(i => MapToDto(i, i.Book.Title, i.Member.Name))
                .ToList();
        }

        // Map Issue to IssueDto
        private static IssueDto MapToDto(Issue i, string bookTitle, string memberName) => new()
        {
            Id = i.Id,
            BookTitle = bookTitle,
            MemberName = memberName,
            IssueDate = i.IssueDate,
            DueDate = i.DueDate,
            ReturnDate = i.ReturnDate,
            Status = i.Status.ToString()
        };
    }
}
