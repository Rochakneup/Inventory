using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; } // Foreign key to Cart
        public int ProductId { get; set; } // Foreign key to Product
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public string ProductName { get; set; } // This property will not be mapped to the database

        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
