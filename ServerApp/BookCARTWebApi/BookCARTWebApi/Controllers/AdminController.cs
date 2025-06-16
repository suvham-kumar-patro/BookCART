using BookCARTWebApi.Data;
using BookCARTWebApi.DTOs;
using BookCARTWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookCARTWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;
        private readonly AppDbContext _context;

        public AdminController(IBookRepository bookRepo, AppDbContext context)
        {
            _bookRepo = bookRepo;
            _context = context;
        }

        // Get pending books with user info
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var books = await _bookRepo.GetPendingApprovalWithUserAsync(); 
            return Ok(books);
        }

        // Approve book
        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var book = await _bookRepo.GetBookByIdAsync(id); 
            if (book == null) return NotFound("Book not found");

            book.IsApproved = true;
            await _bookRepo.SaveAsync(); 
            return Ok(new { message = "Book approved", bookId = book.Id });
        }

        // Reject (delete) book
        [HttpDelete("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var book = await _bookRepo.GetBookByIdAsync(id);
            if (book == null) return NotFound("Book not found");

            await _bookRepo.DeleteBookAsync(id); 
            return Ok(new { message = "Book rejected" });
        }

        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    PhoneNumber = u.PhoneNumber,
                    Role = u.Role
                }).ToListAsync();

            return Ok(users);
        }

        [HttpGet("orders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .ToListAsync();

            var dto = orders.Select(o => new OrderAdminDto
            {
                OrderId = o.Id,
                UserId = o.UserId,
                Username = o.User.Username,
                TotalAmount = o.OrderItems.Sum(oi => oi.Price * oi.Quantity),
                OrderDate = o.OrderDate,
                Items = o.OrderItems.Select(oi => new OrderItemDto
                {
                    BookId = oi.BookId,
                    BookTitle = oi.Book.Title,
                    Price = oi.Price,
                    Quantity = oi.Quantity
                }).ToList()
            });

            return Ok(dto);
        }

    }
}
