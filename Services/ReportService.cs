using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services
{
    // Generates reports for admins
    public class ReportService : IReportService
    {
        private readonly LibraryDbContext _context;

        public ReportService(LibraryDbContext context)
        {
            _context = context;
        }

        // Get the books that have been issued the most times
        public List<BookDto> GetMostBorrowedBooks(int top = 10)
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Issues)
                .OrderByDescending(b => b.Issues.Count)
                .Take(top)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    TotalCopies = b.TotalCopies,
                    AvailableCopies = b.AvailableCopies,
                    AuthorName = b.Author.Name,
                    CategoryName = b.Category.Name
                })
                .ToList();
        }

        // Get members who have borrowed the most books
        public List<MemberDto> GetMostActiveMembers(int top = 10)
        {
            return _context.Members
                .Include(m => m.Issues)
                .OrderByDescending(m => m.Issues.Count)
                .Take(top)
                .Select(m => new MemberDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    Phone = m.Phone,
                    MembershipType = m.MembershipType.ToString(),
                    JoinDate = m.JoinDate
                })
                .ToList();
        }
    }
}
