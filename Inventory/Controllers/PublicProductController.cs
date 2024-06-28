using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Inventory.Controllers
{
    public class PublicProductsController : Controller
    {
        private readonly AuthContext _context;
        private readonly UserManager<AuthUser> _userManager;

        public PublicProductsController(AuthContext context, UserManager<AuthUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: PublicProducts
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Supplier).ToListAsync();
            return View(products); // Renders the Index view with a list of products
        }

        // GET: PublicProducts/Buy/5
        [Authorize]
        public async Task<IActionResult> Buy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // If user is not authenticated, redirect to login
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // Implement buy logic here
                // For now, just redirect to product details
                return RedirectToAction("Details", new { id });
            }
        }
    }
}
