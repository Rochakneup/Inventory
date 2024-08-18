using System;
using System.Collections.Generic;

namespace Inventory.Models
{
    public class Order
    {
        public int Id { get; set; } 
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string? UserFirstName { get; set; } // Added UserFirstName
        public string UserEmail { get; set; } // Added UserEmail
        public string DeliveryAddress { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public enum OrderStatus
    {
        Pending,
        InProgress,
        Shipped,
        Completed,
        Cancelled
    }
}
