namespace BookCARTWebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public required string PhoneNumber { get; set; }
        public string Role { get; set; } = "User"; 
        public List<Book>? Books { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
