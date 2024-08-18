namespace Inventory.Models
{
    public class MergeddataModel
    {

        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string RoleId { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string SupplierName { get; set; }
        public OrderStatus OrderStatus { get; set; }  // Use the enum type here
        public string DeliveryAddress { get; set; }
    }
}
