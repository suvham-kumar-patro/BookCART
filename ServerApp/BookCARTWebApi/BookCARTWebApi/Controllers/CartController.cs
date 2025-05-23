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

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var cartItem = new CartItem
            {
                UserId = userId,
                BookId = dto.BookId,
                Quantity = dto.Quantity,
                Price = dto.Price
            };

            await _cartRepo.AddAsync(cartItem);
            await _cartRepo.SaveAsync();

            return Ok(new { message = "Item added to cart." });
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var items = await _cartRepo.GetCartItemsByUserAsync(userId);
            return Ok(items);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin")] // Optional: Restrict access
        public async Task<IActionResult> GetCartItemsByUserId(int userId)
        {
            var items = await _cartRepo.GetCartItemsByUserAsync(userId);
            return Ok(items);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            await _cartRepo.RemoveFromCartAsync(id);
            return Ok("Item removed");
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _cartRepo.ClearCartAsync(userId);
            return Ok("Cart cleared");
        }
    }

}
