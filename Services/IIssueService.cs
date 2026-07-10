using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services
{
    // Contract for all book issue and return operations
    public interface IIssueService
    {
        IssueDto IssueBook(CreateIssueDto dto);
        (bool success, string message, IssueDto? issue) ReturnBook(int issueId);
        List<IssueDto> GetOverdueIssues();
        List<IssueDto> GetIssuesByMember(int memberId);
    }
}
