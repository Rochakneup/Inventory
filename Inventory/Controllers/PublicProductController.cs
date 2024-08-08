using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Models; // Adjust if the model is in a different namespace
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
        public IActionResult Index(int? categoryId)
        {
            var categories = _context.Categories.ToList(); // Fetch categories from the database or service
            ViewData["Categories"] = categories;

            // Fetch and filter products based on categoryId, etc.
            var products = _context.Products.ToList();
            return View(products);
        }


        // GET: PublicProducts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetPredefinedResponses()
        {
            // Example of retrieving predefined responses from the database
            var responses = await _context.ChatbotResponses.ToListAsync();
            return Json(responses);
        }

        // Other actions as needed...
    }
}
