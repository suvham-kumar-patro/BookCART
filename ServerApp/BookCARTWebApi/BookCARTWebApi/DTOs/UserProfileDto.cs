namespace BookCARTWebApi.DTOs
{
    public class UserProfileDto
    {
        public string Username { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<BookDto> ListedBooks { get; set; } = new();
        public List<OrderDto> OrderHistory { get; set; } = new();
    }
}
