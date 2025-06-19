namespace BookCARTWebApi.DTOs
{
    public class CartItemAdminDto
    {
        public int CartItemId { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public string? Author { get; set; }
    }
}
