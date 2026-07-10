using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    // Handles HTTP requests for fine management
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FineController : ControllerBase
    {
        private readonly IFineService _fineService;

        public FineController(IFineService fineService)
        {
            _fineService = fineService;
        }

        [HttpGet("member/{id}")]          // GET /api/fine/member/1 — pending fines for a member
        public IActionResult GetPendingFines(int id)
        {
            return Ok(_fineService.GetPendingFinesForMember(id));
        }

        [HttpPost("{id}/pay")]            // POST /api/fine/1/pay — mark fine as paid
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult PayFine(int id)
        {
            var (success, message) = _fineService.PayFine(id);
            if (!success) return BadRequest(new { message });
            return Ok(new { message });
        }
    }
}
