using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services
{
    // Handles all member operations using the database
    public class MemberService : IMemberService
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<MemberService> _logger;

        public MemberService(LibraryDbContext context, ILogger<MemberService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<MemberDto> GetAllMembers()
        {
            return _context.Members.Select(m => MapToDto(m)).ToList();
        }

        public MemberDto? GetMemberById(int id)
        {
            var member = _context.Members.Find(id);
            return member == null ? null : MapToDto(member);
        }

        // Get all current and past issues for a member
        public List<IssueDto> GetBorrowedBooks(int memberId)
        {
            return _context.Issues
                .Include(i => i.Book)
                .Include(i => i.Member)
                .Where(i => i.MemberId == memberId)
                .Select(i => MapIssueToDto(i))
                .ToList();
        }

        public MemberDto CreateMember(CreateMemberDto dto)
        {
            var member = new Member
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                MembershipType = dto.MembershipType,
                JoinDate = DateTime.UtcNow
            };
            _context.Members.Add(member);
            _context.SaveChanges();
            _logger.LogInformation("Member registered: {Name}", member.Name);
            return MapToDto(member);
        }

        public MemberDto? UpdateMember(int id, CreateMemberDto dto)
        {
            var member = _context.Members.Find(id);
            if (member == null) return null;

            member.Name = dto.Name;
            member.Email = dto.Email;
            member.Phone = dto.Phone;
            member.MembershipType = dto.MembershipType;
            _context.SaveChanges();
            return MapToDto(member);
        }

        public bool DeleteMember(int id)
        {
            var member = _context.Members.Find(id);
            if (member == null) return false;

            _context.Members.Remove(member);
            _context.SaveChanges();
            return true;
        }

        // Map Member entity to MemberDto
        private static MemberDto MapToDto(Member m) => new()
        {
            Id = m.Id,
            Name = m.Name,
            Email = m.Email,
            Phone = m.Phone,
            MembershipType = m.MembershipType.ToString(),
            JoinDate = m.JoinDate
        };

        // Map Issue entity to IssueDto
        private static IssueDto MapIssueToDto(Issue i) => new()
        {
            Id = i.Id,
            BookTitle = i.Book?.Title ?? "",
            MemberName = i.Member?.Name ?? "",
            IssueDate = i.IssueDate,
            DueDate = i.DueDate,
            ReturnDate = i.ReturnDate,
            Status = i.Status.ToString()
        };
    }
}
