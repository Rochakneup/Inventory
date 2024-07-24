using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

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
        public async Task<IActionResult> Index(int? categoryId)
        {
            var productsQuery = _context.Products.Include(p => p.Supplier).Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            var products = await productsQuery.ToListAsync();
            ViewData["Categories"] = await _context.Categories.ToListAsync(); // Pass categories to the view
            return View(products);
        }



        // GET: PublicProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category) // Assuming Category is a navigation property in the Product model
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
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
