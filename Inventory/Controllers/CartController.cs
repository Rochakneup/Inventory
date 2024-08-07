using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Models;
using Inventory.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Controllers
{
    public class CartController : Controller
    {
        private readonly AuthContext _context;
        private readonly UserManager<AuthUser> _userManager;

        public CartController(AuthContext context, UserManager<AuthUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Adds a product to the user's cart
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["error"] = "You need to be logged in to add items to the cart.";
                return RedirectToAction("Login", "Account");
            }

            // Retrieve or create the cart
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null)
            {
                cart = new Cart { UserId = user.Id };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Retrieve the product
            var product = await _context.Products
                .Where(p => p.Id == productId)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                TempData["error"] = "Product not found.";
                return NotFound();
            }

            // Check if the product is already in the cart
            var cartItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                // Update the existing cart item quantity
                cartItem.Quantity += quantity;
                _context.CartItems.Update(cartItem);
            }
            else
            {
                // Create a new cart item
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    ProductName = product.Name,
                    ProductImageUrl = product.ImageUrl,
                    Quantity = quantity,
                    UnitPrice = product.Price
                };
                cart.CartItems.Add(cartItem);
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            TempData["message"] = "Item added to cart.";
            return RedirectToAction("Index", "PublicProducts");
        }

        // Displays the user's cart
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            return View(cart);
        }

        // Displays the checkout page
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null) return RedirectToAction("Index");

            return View(cart);
        }

        // Handles checkout and creates an order
        [HttpPost]
        public async Task<IActionResult> Checkout(string selectedItems)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["message"] = "Please log in to proceed with checkout.";
                return RedirectToAction("Login", "Account");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null || !cart.CartItems.Any())
            {
                TempData["error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(selectedItems))
            {
                TempData["error"] = "No items selected for checkout.";
                return RedirectToAction("Index");
            }

            var selectedItemsList = selectedItems.Split(',')
                .Select(item =>
                {
                    int itemId;
                    return int.TryParse(item, out itemId) ? (int?)itemId : null;
                })
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();

            if (!selectedItemsList.Any())
            {
                TempData["error"] = "Invalid item selection.";
                return RedirectToAction("Index");
            }

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                UserFirstName = user.Firstname,
                UserEmail = user.Email,
                DeliveryAddress = user.Address,
                OrderItems = new List<OrderItem>()
            };

            foreach (var itemId in selectedItemsList)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == itemId);
                if (cartItem != null)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        UnitPrice = cartItem.UnitPrice
                    };
                    order.OrderItems.Add(orderItem);

                    var product = await _context.Products.FindAsync(cartItem.ProductId);
                    if (product != null)
                    {
                        product.Quantity -= cartItem.Quantity;
                        _context.Products.Update(product);
                    }
                }
                else
                {
                    TempData["error"] = $"Cart item with ID {itemId} was not found.";
                    return RedirectToAction("Index");
                }
            }

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cart.CartItems.Where(ci => selectedItemsList.Contains(ci.Id)));
            if (!cart.CartItems.Any()) _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            TempData["message"] = "Your order has been placed successfully.";
            return RedirectToAction("OrderConfirmation");
        }


        // Removes a product from the user's cart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null) return NotFound();

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null) return NotFound();

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            TempData["message"] = "Item removed from cart successfully.";


            return RedirectToAction("Index");
        }

        // Displays the order confirmation page
        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}
