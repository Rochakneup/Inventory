using Inventory.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class Cart
    {
        public Cart() { 
            CartItems = new List<CartItem>();   

        
        
        }   
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } // Foreign key to AuthUser
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public AuthUser User { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
