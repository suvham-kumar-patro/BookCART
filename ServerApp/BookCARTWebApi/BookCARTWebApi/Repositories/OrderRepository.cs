using BookCARTWebApi.Data;
using BookCARTWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookCARTWebApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> PlaceOrderAsync(Order order, List<CartItem> cartItems)
        {
            order.OrderDate = DateTime.UtcNow;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderItems = cartItems.Select(ci => new OrderItem
            {
                OrderId = order.Id,
                BookId = ci.BookId,
                Quantity = ci.Quantity,
                Price = ci.Price
            });

            _context.OrderItems.AddRange(orderItems);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<List<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .ToListAsync();
        }
    }

}
