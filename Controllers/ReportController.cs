using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    // Admin-only reports for library statistics
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("most-borrowed")]        // GET /api/report/most-borrowed
        public IActionResult GetMostBorrowedBooks([FromQuery] int top = 10)
        {
            return Ok(_reportService.GetMostBorrowedBooks(top));
        }

        [HttpGet("active-members")]       // GET /api/report/active-members
        public IActionResult GetActiveMember([FromQuery] int top = 10)
        {
            return Ok(_reportService.GetMostActiveMembers(top));
        }
    }
}
