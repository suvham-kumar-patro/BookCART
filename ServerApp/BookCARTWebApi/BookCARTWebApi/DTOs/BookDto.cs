namespace BookCARTWebApi.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Stream { get; set; } = string.Empty;
        public string Exam { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public double Price { get; set; }
    }
}
