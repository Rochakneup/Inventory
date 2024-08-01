using Inventory.Models;

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } // Property for product name
    public string ProductImageUrl { get; set; } // Property for image URL
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public Cart Cart { get; set; }
    public Product Product { get; set; }        
}
