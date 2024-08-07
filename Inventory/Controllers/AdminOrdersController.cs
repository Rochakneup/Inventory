using Microsoft.AspNetCore.Mvc;
using Inventory.Models;
using Inventory.Areas.Identity.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Controllers
{
    public class AdminOrdersController : Controller
    {
        private readonly AuthContext _context;

        public AdminOrdersController(AuthContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string status, string searchString)
        {
            var ordersQuery = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse(status, out OrderStatus orderStatus))
                {
                    ordersQuery = ordersQuery.Where(o => o.Status == orderStatus);
                }
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                ordersQuery = ordersQuery.Where(o => o.Id.ToString().Contains(searchString)
                                                    || o.UserFirstName.ToLower().Contains(searchString)
                                                    || o.UserEmail.ToLower().Contains(searchString));
            }

            var orders = await ordersQuery.ToListAsync();
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentStatus = status;

            return View(orders);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            var orderToUpdate = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orderToUpdate == null)
            {
                return NotFound();
            }

            // Check for model state validity
            if (ModelState.IsValid)
            {
                return View(orderToUpdate);
            }

            // Handle concurrency issues
            try
            {
                orderToUpdate.Status = order.Status;
                orderToUpdate.DeliveryDate = order.DeliveryDate;
                orderToUpdate.DeliveryAddress = order.DeliveryAddress;

                TempData["message"] = "Order edited.";


                _context.Orders.Update(orderToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            Console.WriteLine($"Order ID: {order.Id}");
            Console.WriteLine($"Number of Order Items: {order.OrderItems.Count}");
            foreach (var item in order.OrderItems)
            {
                Console.WriteLine($"Product ID: {item.ProductId}, Product Name: {item.Product?.Name}");
            }

            return View(order);
        }



        [HttpPost]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = OrderStatus.Cancelled;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            // Redirect to Home/Index
            return RedirectToAction("Index", "PublicProducts");
        }









        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}