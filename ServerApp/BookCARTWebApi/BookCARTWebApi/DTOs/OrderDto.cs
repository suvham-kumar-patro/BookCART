namespace BookCARTWebApi.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
