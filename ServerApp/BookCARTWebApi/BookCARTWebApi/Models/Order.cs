using System.ComponentModel.DataAnnotations.Schema;

namespace BookCARTWebApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Foreign Keys
        //[ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
