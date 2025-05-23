using BookCARTWebApi.Models;

namespace BookCARTWebApi.Repositories
{
    public interface ICartRepository
    {
        Task AddToCartAsync(CartItem cartItem);
        Task<List<CartItem>> GetCartItemsByUserAsync(int userId);
        Task RemoveFromCartAsync(int cartItemId);
        Task ClearCartAsync(int userId);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task AddAsync(CartItem cartItem);
        Task SaveAsync();
    }
}
