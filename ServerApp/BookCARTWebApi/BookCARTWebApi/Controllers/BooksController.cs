using BookCARTWebApi.DTOs;
using BookCARTWebApi.Models;
using BookCARTWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> SellBook([FromForm] BookUploadDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            string imageUrl = string.Empty;

            if (dto.Image != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                imageUrl = "/uploads/" + uniqueFileName;
            }

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = imageUrl, 
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

            return Ok(new { message = "Book submitted for approval" });
        }

        //[HttpPost("sell")]
        //[Authorize]
        //public async Task<IActionResult> SellBook(BookDto dto)
        //{
        //    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        //    var book = new Book
        //    {
        //        Title = dto.Title,
        //        Author = dto.Author,
        //        Publisher = dto.Publisher,
        //        Description = dto.Description,
        //        Price = dto.Price,
        //        ImageUrl = dto.ImageUrl,
        //        Category = dto.Category,
        //        Stream = dto.Stream,
        //        Exam = dto.Exam,
        //        Subject = dto.Subject,
        //        Language = dto.Language,
        //        Format = dto.Format,
        //        Condition = dto.Condition,
        //        PublicationYear = dto.PublicationYear,
        //        IsApproved = false,
        //        UserId = userId
        //    };
        //    await _bookRepo.AddAsync(book);
        //    await _bookRepo.SaveAsync();
        //    return Ok("Book submitted for approval");
        //}

        [HttpGet("filter")]
        public async Task<IActionResult> FilterBooks(
            [FromQuery] string? search,
            [FromQuery] string? category,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] string? format)
        {
            var filteredBooks = await _bookRepo.FilterBooksAsync(search, category, minPrice, maxPrice, format);
            return Ok(filteredBooks);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBook(int id, [FromForm] BookUpdateDto dto)
        {
            var existingBook = await _bookRepo.GetBookByIdAsync(id);
            if (existingBook == null)
                return NotFound("Book not found.");

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                return Unauthorized();

            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userRole != "Admin" && existingBook.UserId != userId)
                return Forbid("You are not authorized to update this book.");

            // Handle optional image upload
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                existingBook.ImageUrl = "/uploads/" + uniqueFileName;
            }


            //if (dto.Image != null && dto.Image.Length > 0)
            //{
            //    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            //    if (!Directory.Exists(uploadsFolder))
            //        Directory.CreateDirectory(uploadsFolder);

            //    var uniqueFileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            //    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            //    using var stream = new FileStream(filePath, FileMode.Create);
            //    await dto.Image.CopyToAsync(stream);

            //    existingBook.ImageUrl = "/uploads/" + uniqueFileName;
            //}
            //else if (!string.IsNullOrEmpty(dto.ImageUrl))
            //{
            //    existingBook.ImageUrl = dto.ImageUrl;
            //}

            // Conditionally update fields
            if (!string.IsNullOrEmpty(dto.Title)) existingBook.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Author)) existingBook.Author = dto.Author;
            if (!string.IsNullOrEmpty(dto.Description)) existingBook.Description = dto.Description;
            if (!string.IsNullOrEmpty(dto.Category)) existingBook.Category = dto.Category;
            if (!string.IsNullOrEmpty(dto.Stream)) existingBook.Stream = dto.Stream;
            if (!string.IsNullOrEmpty(dto.Exam)) existingBook.Exam = dto.Exam;
            if (!string.IsNullOrEmpty(dto.Subject)) existingBook.Subject = dto.Subject;
            if (!string.IsNullOrEmpty(dto.Language)) existingBook.Language = dto.Language;
            if (!string.IsNullOrEmpty(dto.Format)) existingBook.Format = dto.Format;
            if (!string.IsNullOrEmpty(dto.Condition)) existingBook.Condition = dto.Condition;
            if (dto.PublicationYear.HasValue) existingBook.PublicationYear = dto.PublicationYear.Value;
            if (dto.Price.HasValue) existingBook.Price = dto.Price.Value;
            if (dto.IsApproved.HasValue) existingBook.IsApproved = dto.IsApproved.Value;

            await _bookRepo.SaveAsync();

            return Ok(new { message = "Book updated successfully." });
        }




        //[HttpPut("{id}")]
        //[Authorize]
        //public async Task<IActionResult> UpdateBook(int id, [FromForm] BookUploadDto dto)
        //{
        //    var existingBook = await _bookRepo.GetBookByIdAsync(id);
        //    if (existingBook == null)
        //        return NotFound("Book not found.");

        //    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        //    var userRole = User.FindFirstValue(ClaimTypes.Role); // or use "role" if that's what you're using

        //    // 🛡️ Only block non-admins from editing other users' books
        //    if (userRole != "Admin" && existingBook.UserId != userId)
        //        return Forbid("You are not authorized to update this book.");

        //    // ✅ Handle image upload
        //    if (dto.Image != null && dto.Image.Length > 0)
        //    {
        //        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        //        if (!Directory.Exists(uploadsFolder))
        //            Directory.CreateDirectory(uploadsFolder);

        //        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
        //        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await dto.Image.CopyToAsync(stream);
        //        }

        //        existingBook.ImageUrl = "/uploads/" + uniqueFileName;
        //    }

        //    if (!string.IsNullOrEmpty(dto.Title)) book.Title = dto.Title;
        //    if (!string.IsNullOrEmpty(dto.Author)) book.Author = dto.Author;
        //    if (!string.IsNullOrEmpty(dto.Publisher)) book.Publisher = dto.Publisher;
        //    if (!string.IsNullOrEmpty(dto.Description)) book.Description = dto.Description;
        //    if (!string.IsNullOrEmpty(dto.Category)) book.Category = dto.Category;
        //    if (!string.IsNullOrEmpty(dto.Stream)) book.Stream = dto.Stream;
        //    if (!string.IsNullOrEmpty(dto.Exam)) book.Exam = dto.Exam;
        //    if (!string.IsNullOrEmpty(dto.Subject)) book.Subject = dto.Subject;
        //    if (!string.IsNullOrEmpty(dto.Language)) book.Language = dto.Language;
        //    if (!string.IsNullOrEmpty(dto.Format)) book.Format = dto.Format;
        //    if (!string.IsNullOrEmpty(dto.Condition)) book.Condition = dto.Condition;
        //    if (!string.IsNullOrEmpty(dto.ImageUrl)) book.ImageUrl = dto.ImageUrl;

        //    if (dto.PublicationYear.HasValue) book.PublicationYear = dto.PublicationYear.Value;
        //    if (dto.Price.HasValue) book.Price = dto.Price.Value;
        //    if (dto.IsApproved.HasValue) book.IsApproved = dto.IsApproved.Value;

        //    await _bookRepo.SaveAsync();

        // ✏️ Update other fields
        //existingBook.Title = dto.Title;
        //existingBook.Description = dto.Description;
        //existingBook.Author = dto.Author;
        //existingBook.Publisher = dto.Publisher;
        //existingBook.Category = dto.Category;
        //existingBook.Stream = dto.Stream;
        //existingBook.Exam = dto.Exam;
        //existingBook.Subject = dto.Subject;
        //existingBook.Language = dto.Language;
        //existingBook.Format = dto.Format;
        //existingBook.Condition = dto.Condition;
        //existingBook.PublicationYear = dto.PublicationYear;
        //existingBook.Price = dto.Price;

        //    await _bookRepo.SaveAsync();

        //    return Ok(new { message = "Book updated successfully." });
        //}


        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateDto dto)
        //{
        //    var existingBook = await _bookRepo.GetBookByIdAsync(id);
        //    if (existingBook == null)
        //        return NotFound("Book not found.");

        //    await _bookRepo.UpdateBookAsync(id, dto);
        //    return Ok(new { message = "Book updated successfully." });
        //}

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
