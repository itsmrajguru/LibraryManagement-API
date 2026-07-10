using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    // Handles HTTP requests for issuing and returning books
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssueController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpPost]                        // POST /api/issue — issue a book to a member
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult IssueBook([FromBody] CreateIssueDto dto)
        {
            var issue = _issueService.IssueBook(dto);
            return CreatedAtAction(null, issue);
        }

        [HttpPut("{id}/return")]          // PUT /api/issue/1/return — mark book as returned
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult ReturnBook(int id)
        {
            var (success, message, issue) = _issueService.ReturnBook(id);
            if (!success) return BadRequest(new { message });
            return Ok(new { message, issue });
        }

        [HttpGet("overdue")]              // GET /api/issue/overdue — books past due date
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult GetOverdueIssues()
        {
            return Ok(_issueService.GetOverdueIssues());
        }

        [HttpGet("member/{memberId}")]    // GET /api/issue/member/1 — all issues for a member
        public IActionResult GetIssuesByMember(int memberId)
        {
            return Ok(_issueService.GetIssuesByMember(memberId));
        }
    }
}
