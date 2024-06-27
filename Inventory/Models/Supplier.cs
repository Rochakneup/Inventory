namespace Inventory.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
