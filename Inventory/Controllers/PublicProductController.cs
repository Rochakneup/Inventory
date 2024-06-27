using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Areas.Identity.Data;

namespace Inventory.Controllers
{
    public class PublicProductsController : Controller
    {
        private readonly AuthContext _context;

        public PublicProductsController(AuthContext context)
        {
            _context = context;
        }

        // GET: PublicProducts
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Supplier).ToListAsync();
            return View(products); // Renders the Index view with a list of products
        }
    }
}
