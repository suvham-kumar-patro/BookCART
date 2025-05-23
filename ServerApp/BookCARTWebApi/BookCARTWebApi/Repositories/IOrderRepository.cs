using BookCARTWebApi.Models;

namespace BookCARTWebApi.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> PlaceOrderAsync(Order order, List<CartItem> cartItems);
        Task<List<Order>> GetOrdersByUserAsync(int userId);
    }

}
