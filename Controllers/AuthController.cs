using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    // Handles user registration and login — no auth required here
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]            // POST /api/auth/register
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            var result = _authService.Register(dto);
            return Ok(result);
        }

        [HttpPost("login")]               // POST /api/auth/login — returns JWT token
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var result = _authService.Login(dto);
            return Ok(result);
        }
    }
}
