using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services
{
    // Contract for all member-related operations
    public interface IMemberService
    {
        List<MemberDto> GetAllMembers();
        MemberDto? GetMemberById(int id);
        List<IssueDto> GetBorrowedBooks(int memberId);
        MemberDto CreateMember(CreateMemberDto dto);
        MemberDto? UpdateMember(int id, CreateMemberDto dto);
        bool DeleteMember(int id);
    }
}
