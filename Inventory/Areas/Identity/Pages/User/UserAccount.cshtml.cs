using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Areas.Identity.Data;
using Inventory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Areas.Identity.Pages.User
{
    public class UserAccountModel : PageModel
    {
        private readonly AuthContext _context;
        private readonly UserManager<AuthUser> _userManager;

        public UserAccountModel(AuthContext context, UserManager<AuthUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Order> Orders { get; set; } = new List<Order>();
        public int PendingOrdersCount { get; set; }
        public int CompletedOrdersCount { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product) // Include Product details
                    .Where(o => o.UserEmail == user.Email)
                    .ToListAsync();

                PendingOrdersCount = Orders.Count(o => o.Status == OrderStatus.Pending);
                CompletedOrdersCount = Orders.Count(o => o.Status == OrderStatus.Completed);
            }
        }
    }
}
