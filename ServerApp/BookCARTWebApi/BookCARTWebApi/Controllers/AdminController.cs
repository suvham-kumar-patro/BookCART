using BookCARTWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCARTWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;

        public AdminController(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }

        // Get pending books with user info
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var books = await _bookRepo.GetPendingApprovalWithUserAsync(); // Ensure this method exists
            return Ok(books);
        }

        // Approve book
        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var book = await _bookRepo.GetBookByIdAsync(id); // Ensure this includes User
            if (book == null) return NotFound("Book not found");

            book.IsApproved = true;
            await _bookRepo.SaveAsync(); // SaveAsync after changing state
            return Ok("Book approved");
        }

        // Reject (delete) book
        [HttpDelete("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var book = await _bookRepo.GetBookByIdAsync(id);
            if (book == null) return NotFound("Book not found");

            await _bookRepo.DeleteBookAsync(id); // Prefer async delete method
            return Ok("Book rejected");
        }
    }
}
