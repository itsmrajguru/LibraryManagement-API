using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services
{
    // Contract for register and login operations
    public interface IAuthService
    {
        AuthResponseDto Register(RegisterDto dto);
        AuthResponseDto Login(LoginDto dto);
    }
}
