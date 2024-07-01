using Inventory.Areas.Identity.Data;
using Inventory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        public List<Supplier> Suppliers { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ActiveUsersCount = await _context.Users.CountAsync(u => u.Status == "Active");
            LowStockProductsCount = await _context.Products.CountAsync(p => p.Quantity <= 10);
            SuppliersCount = await _context.Suppliers.CountAsync();
            Suppliers = await _context.Suppliers.Include(s => s.Products).ToListAsync();

            return Page();
        }
    }
}