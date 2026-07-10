using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    // Handles HTTP requests for member management
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]                         // GET /api/member — Admin/Librarian only
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult GetAllMembers()
        {
            return Ok(_memberService.GetAllMembers());
        }

        [HttpGet("{id}")]                 // GET /api/member/1
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult GetMemberById(int id)
        {
            var member = _memberService.GetMemberById(id);
            if (member == null) return NotFound(new { message = $"Member with ID {id} not found!" });
            return Ok(member);
        }

        [HttpGet("{id}/borrowed-books")]  // GET /api/member/1/borrowed-books
        public IActionResult GetBorrowedBooks(int id)
        {
            return Ok(_memberService.GetBorrowedBooks(id));
        }

        [HttpPost]                        // POST /api/member — Admin/Librarian only
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult CreateMember([FromBody] CreateMemberDto dto)
        {
            var member = _memberService.CreateMember(dto);
            return CreatedAtAction(nameof(GetMemberById), new { id = member.Id }, member);
        }

        [HttpPut("{id}")]                 // PUT /api/member/1 — Admin/Librarian only
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult UpdateMember(int id, [FromBody] CreateMemberDto dto)
        {
            var member = _memberService.UpdateMember(id, dto);
            if (member == null) return NotFound(new { message = $"Member with ID {id} not found!" });
            return Ok(member);
        }

        [HttpDelete("{id}")]              // DELETE /api/member/1 — Admin only
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMember(int id)
        {
            var success = _memberService.DeleteMember(id);
            if (!success) return NotFound(new { message = $"Member with ID {id} not found!" });
            return NoContent();
        }
    }
}
