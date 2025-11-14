using Microsoft.AspNetCore.Mvc;

namespace BookCARTWebApi.DTOs
{
    public class BookUploadDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Stream { get; set; } = string.Empty;
        public string? Exam { get; set; } = string.Empty;
        public string? Subject { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public double Price { get; set; }

        [FromForm(Name = "Image")]
        public IFormFile Image { get; set; }  
    }
}
