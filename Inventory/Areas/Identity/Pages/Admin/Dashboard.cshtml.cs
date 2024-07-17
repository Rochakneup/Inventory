using Inventory.Areas.Identity.Data;
using Inventory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Areas.Identity.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly AuthContext _context;

        public DashboardModel(AuthContext context)
        {
            _context = context;
        }

        public int ActiveUsersCount { get; set; }
        public int LowStockProductsCount { get; set; }
        public int SuppliersCount { get; set; }
        public int MonthlyOrdersCount { get; set; }

        // Properties for charts
        public List<StatusData> OrdersByStatus { get; set; }
        public List<SupplierData> ProductsBySupplier { get; set; }
        public List<CategoryData> ProductsByCategory { get; set; }
        public ProductStatusData ProductStatus { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ActiveUsersCount = await _context.Users.CountAsync(u => u.Status == "Active");
            LowStockProductsCount = await _context.Products.CountAsync(p => p.Quantity <= 10);
            SuppliersCount = await _context.Suppliers.CountAsync();

            // Monthly Orders Count
            var currentMonth = DateTime.Now.Month;
            MonthlyOrdersCount = await _context.Orders.CountAsync(o => o.OrderDate.Month == currentMonth);

            // Orders by Status
            var orders = await _context.Orders.ToListAsync();
            OrdersByStatus = orders
                .GroupBy(o => o.Status)
                .Select(g => new StatusData
                {
                    Status = g.Key.ToString(), // Convert to string here
                    Count = g.Count()
                })
                .OrderBy(e0 => e0.Status)
                .ToList();

            // Products by Supplier
            var products = await _context.Products.Include(p => p.Supplier).ToListAsync();
            ProductsBySupplier = products
                .GroupBy(p => p.Supplier?.Name ?? "No Supplier") // Handle null Supplier
                .Select(g => new SupplierData
                {
                    SupplierName = g.Key,
                    ProductCount = g.Count()
                })
                .OrderBy(d => d.SupplierName)
                .ToList();

            // Products by Category
            ProductsByCategory = products
                .GroupBy(p => p.Category?.Name ?? "No Category") // Handle null Category
                .Select(g => new CategoryData
                {
                    CategoryName = g.Key,
                    ProductCount = g.Count()
                })
                .OrderBy(d => d.CategoryName)
                .ToList();

            // Product Status
            var productStatuses = new Dictionary<string, int>
    {
        { "Available", 0 },
        { "Low Stock", 0 },
        { "Out of Stock", 0 }
    };

            foreach (var product in products)
            {
                if (product.Quantity == 0)
                {
                    productStatuses["Out of Stock"]++;
                }
                else if (product.Quantity <= 10)
                {
                    productStatuses["Low Stock"]++;
                }
                else
                {
                    productStatuses["Available"]++;
                }
            }

            ProductStatus = new ProductStatusData
            {
                Available = productStatuses["Available"],
                LowStock = productStatuses["Low Stock"],
                OutOfStock = productStatuses["Out of Stock"]
            };

            return Page();
        }


    }

    public class StatusData
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class SupplierData
    {
        public string SupplierName { get; set; }
        public int ProductCount { get; set; }
    }

    public class CategoryData
    {
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }
    }

    public class ProductStatusData
    {
        public int Available { get; set; }
        public int LowStock { get; set; }
        public int OutOfStock { get; set; }
    }
}
