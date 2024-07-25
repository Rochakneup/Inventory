namespace Inventory.Models
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string UserEmail { get; set; }
        public string UserFirstName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int SupplierId { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }

}
