namespace BookCARTWebApi.DTOs
{
    public class BookUpdateDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Stream {  get; set; }
        public string? Exam {  get; set; }
        public string? Subject { get; set; }
        public string? Language { get; set; }
        public string? Format { get; set; }
        public double? Price { get; set; }
        public IFormFile? Image { get; set; }
        public string? Condition { get; set; }
        public int? PublicationYear { get; set; }
        public bool? IsApproved { get; set; }
    }
}
