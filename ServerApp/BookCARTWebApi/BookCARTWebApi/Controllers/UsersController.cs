using BookCARTWebApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BookCARTWebApi.Models;
using BookCARTWebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BookCARTWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound();

            var listedBooks = await _context.Books
                .Where(b => b.UserId == userId)
                .ToListAsync();

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .ToListAsync();

            var profile = new UserProfileDto
            {
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                ListedBooks = listedBooks.Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    Description = b.Description,
                    Author = b.Author,
                    Category = b.Category,
                    Stream = b.Stream,
                    Exam = b.Exam,
                    Subject = b.Subject,
                    Language = b.Language,
                    Condition = b.Condition,
                    Format = b.Format,
                    PublicationYear = b.PublicationYear,
                    Price = b.Price
                }).ToList(),
                OrderHistory = orders.Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.OrderItems.Sum(oi => oi.Quantity * oi.Price),
                    Items = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        BookId = oi.BookId,
                        BookTitle = oi.Book.Title,
                        Quantity = oi.Quantity,
                        Price = oi.Price
                    }).ToList()
                }).ToList()
            };

            return Ok(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.PhoneNumber = dto.PhoneNumber;
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
