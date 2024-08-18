using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;

namespace Inventory.Services
{
    public class DashboardService
    {
        private readonly AuthContext _context;

        public DashboardService(AuthContext context)
        {
            _context = context;
        }

        public List<MergeddataModel> GetMergedData()
        {
            // Example query to fetch and merge data
            var mergedData = from user in _context.Users
                             join role in _context.UserRoles on user.Id equals role.UserId into userRoles
                             from userRole in userRoles.DefaultIfEmpty()
                             join order in _context.Orders on user.Id equals order.UserFirstName
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
                             };

            return mergedData.ToList();
        }

    }
}
