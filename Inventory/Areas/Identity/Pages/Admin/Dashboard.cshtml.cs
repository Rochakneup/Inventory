using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Inventory.Areas.Identity.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly AuthContext _context;
        private readonly GptService _gptService;

        public DashboardModel(AuthContext context, GptService gptService)
        {
            _context = context;
            _gptService = gptService;
        }

        // Properties for merged data and GPT API
        public List<MergeddataModel> MergedData { get; set; }
        [BindProperty]
        public string SearchQuery { get; set; }
        public string GptApiResponse { get; set; }

        // Properties for admin dashboard statistics and charts
        public int ActiveUsersCount { get; set; }
        public int LowStockProductsCount { get; set; }
        public int SuppliersCount { get; set; }
        public int MonthlyOrdersCount { get; set; }
        public List<StatusData> OrdersByStatus { get; set; }
        public List<SupplierData> ProductsBySupplier { get; set; }
        public List<CategoryData> ProductsByCategory { get; set; }
        public ProductStatusData ProductStatus { get; set; }

        public async Task OnGetAsync()
        {
            await LoadMergedDataAsync();
            await LoadDashboardStatisticsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadMergedDataAsync();

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                var filteredData = MergedData
                    .Where(md => md.UserName.Contains(SearchQuery) || md.ProductName.Contains(SearchQuery))
                    .Select(md => $"{md.UserName} bought {md.ProductName} on {md.OrderDate}")
                    .ToList();

                GptApiResponse = await _gptService.GetGptResponseAsync(SearchQuery, filteredData);
            }

            await LoadDashboardStatisticsAsync();
            return Page();
        }

        private async Task LoadMergedDataAsync()
        {
            MergedData = await (from user in _context.Users
                                join role in _context.UserRoles on user.Id equals role.UserId into userRoles
                                from userRole in userRoles.DefaultIfEmpty()
                                join order in _context.Orders on user.UserName equals order.UserFirstName
                                join orderItem in _context.OrderItems on order.Id equals orderItem.OrderId
                                join product in _context.Products on orderItem.ProductId equals product.Id
                                join category in _context.Categories on product.CategoryId equals category.Id
                                join supplier in _context.Suppliers on product.SupplierId equals supplier.Id
                                select new MergeddataModel
                                {
                                    UserName = user.UserName,
                                    UserEmail = user.Email,
                                    RoleId = userRole.RoleId,
                                    CategoryName = category.Name,
                                    ProductName = product.Name,
                                    Quantity = orderItem.Quantity,
                                    UnitPrice = orderItem.UnitPrice,
                                    OrderDate = order.OrderDate,
                                    SupplierName = supplier.Name,
                                    OrderStatus = order.Status,
                                    DeliveryAddress = order.DeliveryAddress
                                }).ToListAsync();
        }

        private async Task LoadDashboardStatisticsAsync()
        {
            ActiveUsersCount = await _context.Users.CountAsync(u => u.Status == "Active");
            LowStockProductsCount = await _context.Products.CountAsync(p => p.Quantity <= 10);
            SuppliersCount = await _context.Suppliers.CountAsync();

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var startDate = new DateTime(currentYear, currentMonth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var previousMonthStartDate = startDate.AddMonths(-1);
            var previousMonthEndDate = previousMonthStartDate.AddMonths(1).AddDays(-1);

            MonthlyOrdersCount = await _context.Orders.CountAsync(o =>
                (o.OrderDate >= startDate && o.OrderDate <= endDate) ||
                (o.OrderDate >= previousMonthStartDate && o.OrderDate <= previousMonthEndDate)
            );

            var orders = await _context.Orders
                .Where(o =>
                    (o.OrderDate >= startDate && o.OrderDate <= endDate) ||
                    (o.OrderDate >= previousMonthStartDate && o.OrderDate <= previousMonthEndDate)
                )
                .ToListAsync();

            OrdersByStatus = orders
                .GroupBy(o => o.Status)
                .Select(g => new StatusData
                {
                    Status = g.Key.ToString(),
                    Count = g.Count()
                })
                .OrderBy(e0 => e0.Status)
                .ToList();

            var products = await _context.Products.Include(p => p.Supplier).Include(p => p.Category).ToListAsync();

            ProductsBySupplier = products
                .GroupBy(p => p.Supplier?.Name ?? "No Supplier")
                .Select(g => new SupplierData
                {
                    SupplierName = g.Key,
                    ProductCount = g.Count()
                })
                .OrderBy(d => d.SupplierName)
                .ToList();

            ProductsByCategory = products
                .GroupBy(p => p.Category?.Name ?? "No Category")
                .Select(g => new CategoryData
                {
                    CategoryName = g.Key,
                    ProductCount = g.Count()
                })
                .OrderBy(d => d.CategoryName)
                .ToList();

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
}
