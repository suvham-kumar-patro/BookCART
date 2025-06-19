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
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepo;

        public CartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        // POST: api/Cart/add
        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto dto)
        {
            if (dto.Quantity < 1)
            {
                return BadRequest(new { message = "Quantity must be at least 1." });
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var existingCartItem = await _cartRepo.GetCartItemAsync(userId, dto.BookId);

            if (existingCartItem != null)
            {
                // ✅ Add to existing quantity
                existingCartItem.Quantity += dto.Quantity;
                await _cartRepo.UpdateCartItemAsync(existingCartItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    UserId = userId,
                    BookId = dto.BookId,
                    Quantity = dto.Quantity,
                    Price = dto.Price
                };
                await _cartRepo.AddToCartAsync(cartItem);
            }

            return Ok(new { message = "Item added/updated in cart." });
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCartItems()
        {
            var items = await _cartRepo.GetAllCartItemsAsync();

            var dtoItems = items.Select(item => new CartItemAdminDto
            {
                CartItemId = item.Id,
                Username = item.User?.Username,
                PhoneNumber = item.User?.PhoneNumber,
                UserId = item.UserId,
                BookId = item.BookId,
                Title = item.Book?.Title,
                ImageUrl = item.Book?.ImageUrl,
                Price = item.Price,
                Quantity = item.Quantity,
                Author  = item.Book?.Author
            });

            return Ok(dtoItems);
        }


        // GET: api/Cart
        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var items = await _cartRepo.GetCartItemsByUserAsync(userId);

            var dtoItems = items.Select(item => new CartItemDto
            {
                Id = item.Id,
                BookId = item.BookId,
                Title = item.Book?.Title ?? "Unknown",
                ImageUrl = item.Book?.ImageUrl ?? string.Empty,
                Price = item.Price,
                Quantity = item.Quantity,
                Author = item.Book?.Author ?? "Unknown"
            }).ToList();

            return Ok(dtoItems);
        }


        // GET: api/Cart/{userId}
        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCartItemsByUserId(int userId)
        {
            var items = await _cartRepo.GetCartItemsByUserAsync(userId);

            var dtoItems = items.Select(item => new CartItemDto
            {
                Id = item.Id,
                BookId = item.BookId,
                Title = item.Book?.Title,
                ImageUrl = item.Book?.ImageUrl,
                Price = item.Price,
                Quantity = item.Quantity,
                Author = item.Book?.Author
            });

            return Ok(dtoItems);
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity([FromBody] CartItemDto dto)
        {
            if (dto.Quantity < 1)
                return BadRequest(new { message = "Quantity must be at least 1." });

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var existingCartItem = await _cartRepo.GetCartItemAsync(userId, dto.BookId);

            if (existingCartItem == null)
                return NotFound(new { message = "Item not found in cart." });

            existingCartItem.Quantity = dto.Quantity;
            await _cartRepo.UpdateCartItemAsync(existingCartItem);

            return Ok(new { message = "Quantity updated." });
        }


        // DELETE: api/Cart/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            await _cartRepo.RemoveFromCartAsync(id);
            return Ok(new { message = "Item removed from cart." });
        }

        // DELETE: api/Cart/clear
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _cartRepo.ClearCartAsync(userId);
            return Ok(new { message = "Cart cleared." });
        }
    }
}
