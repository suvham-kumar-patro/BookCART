namespace BookCARTWebApi.DTOs
{
    public class OrderAdminDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public double TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
