using BookCARTWebApi.DTOs;
using BookCARTWebApi.Models;
using BookCARTWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookCARTWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;

        public BooksController(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetApprovedBooks()
        {
            var books = await _bookRepo.GetApprovedBooksWithUserDtoAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookRepo.GetBookByIdAsync(id);
            if (book == null) return NotFound();

            var dto = new BookWithUserDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Category = book.Category,
                Language = book.Language,
                Format = book.Format,
                Price = book.Price,
                ImageUrl = book.ImageUrl,
                Condition = book.Condition,
                PublicationYear = book.PublicationYear,
                IsApproved = book.IsApproved,
                UserName = book.User?.Username,
                PhoneNumber = book.User?.PhoneNumber
            };

            return Ok(dto);
        }

        [HttpPost("sell")]
        [Authorize]
        public async Task<IActionResult> SellBook(BookDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Publisher = dto.Publisher,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                Category = dto.Category,
                Stream = dto.Stream,
                Exam = dto.Exam,
                Subject = dto.Subject,
                Language = dto.Language,
                Format = dto.Format,
                Condition = dto.Condition,
                PublicationYear = dto.PublicationYear,
                IsApproved = false,
                UserId = userId
            };
            await _bookRepo.AddAsync(book);
            await _bookRepo.SaveAsync();
            return Ok("Book submitted for approval");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateDto dto)
        {
            var existingBook = await _bookRepo.GetBookByIdAsync(id);
            if (existingBook == null)
                return NotFound("Book not found.");

            await _bookRepo.UpdateBookAsync(id, dto);
            return Ok(new { message = "Book updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepo.GetBookByIdAsync(id);
            if (book == null)
                return NotFound("Book not found.");

            await _bookRepo.DeleteBookAsync(id);
            return Ok(new { message = "Book deleted successfully." });
        }
    }

}
