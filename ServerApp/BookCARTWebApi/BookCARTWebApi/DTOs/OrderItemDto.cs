
namespace BookCARTWebApi.DTOs
{
    public class OrderItemDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
