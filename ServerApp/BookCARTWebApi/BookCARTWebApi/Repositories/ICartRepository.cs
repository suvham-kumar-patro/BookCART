using BookCARTWebApi.Models;

public interface ICartRepository
{
    Task AddToCartAsync(CartItem cartItem);
    Task<CartItem?> GetCartItemAsync(int userId, int bookId);
    Task<List<CartItem>> GetAllCartItemsAsync();
    Task<List<CartItem>> GetCartItemsByUserAsync(int userId);
    Task RemoveFromCartAsync(int cartItemId);
    Task ClearCartAsync(int userId);
    Task UpdateCartItemAsync(CartItem cartItem);
    Task SaveAsync(); 
}
