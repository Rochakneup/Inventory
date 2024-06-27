using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class OrderViewModel
    {
        public List<OrderItemViewModel> OrderItems { get; set; }

        // You can add additional fields here as needed
    }

    public class OrderItemViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
