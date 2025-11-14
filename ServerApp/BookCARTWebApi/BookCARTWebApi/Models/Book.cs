using System.ComponentModel.DataAnnotations.Schema;

namespace BookCARTWebApi.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string? MainCategory { get; set; }      
        public string? SchoolClass { get; set; }      
        public string? Board { get; set; }           
        public string? CollegeLevel { get; set; }     
        public string? Course { get; set; }         
        public string? Honors { get; set; }           
        public string? MedicalCourse { get; set; }    
        public string? OthersCategory { get; set; }   

        public string Stream { get; set; } = string.Empty; 
        public string Exam { get; set; } = string.Empty;    
        public string Subject { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;  
        public int PublicationYear { get; set; }
        public double Price { get; set; }
        public bool IsApproved { get; set; } = false;

        // Foreign Key
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
