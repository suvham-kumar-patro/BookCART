namespace BookCARTWebApi.DTOs
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? Author { get; set; }


    }

}
