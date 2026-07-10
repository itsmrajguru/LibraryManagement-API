using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services
{
    // Contract for all fine-related operations
    public interface IFineService
    {
        List<FineDto> GetPendingFinesForMember(int memberId);
        (bool success, string message) PayFine(int fineId);
    }
}
