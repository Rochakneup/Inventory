using System;
using System.Collections.Generic;

namespace Inventory.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string UserId { get; set; } // This should match the type of your user ID

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public enum OrderStatus
    {
        Pending,
        InProgress,
        Shipped,
        Completed,
        Cancelled
        // Add more status options as needed
    }
}
