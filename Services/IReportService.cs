using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services
{
    // Contract for admin report operations
    public interface IReportService
    {
        List<BookDto> GetMostBorrowedBooks(int top = 10);
        List<MemberDto> GetMostActiveMembers(int top = 10);
    }
}
