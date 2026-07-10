using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services
{
    // Handles all fine payment operations
    public class FineService : IFineService
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<FineService> _logger;

        public FineService(LibraryDbContext context, ILogger<FineService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Get all unpaid fines for a specific member
        public List<FineDto> GetPendingFinesForMember(int memberId)
        {
            return _context.Fines
                .Include(f => f.Issue).ThenInclude(i => i.Book)
                .Include(f => f.Issue).ThenInclude(i => i.Member)
                .Where(f => f.Issue.MemberId == memberId && !f.IsPaid)
                .Select(f => MapToDto(f))
                .ToList();
        }

        // Mark a fine as paid
        public (bool success, string message) PayFine(int fineId)
        {
            var fine = _context.Fines.Find(fineId);
            if (fine == null) return (false, "Fine not found!");
            if (fine.IsPaid) return (false, "Fine is already paid!");

            fine.IsPaid = true;
            fine.PaidDate = DateTime.UtcNow;
            _context.SaveChanges();
            _logger.LogInformation("Fine {FineId} of ₹{Amount} paid", fineId, fine.Amount);

            return (true, $"Fine of ₹{fine.Amount} paid successfully!");
        }

        // Map Fine entity to FineDto
        private static FineDto MapToDto(Models.Fine f) => new()
        {
            Id = f.Id,
            IssueId = f.IssueId,
            BookTitle = f.Issue?.Book?.Title ?? "",
            MemberName = f.Issue?.Member?.Name ?? "",
            Amount = f.Amount,
            IsPaid = f.IsPaid,
            PaidDate = f.PaidDate
        };
    }
}
