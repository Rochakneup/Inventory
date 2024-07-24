// CartController.cs

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
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null)
            {
                cart = new Cart { UserId = user.Id };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
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
            }

            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Displays the user's cart
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            return View(cart);
        }

        // Handles checkout and creates an order
        [HttpPost]
        public async Task<IActionResult> Checkout(int[] selectedItems)
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null) return NotFound();

            // Log cart details
            Console.WriteLine($"Cart contains {cart.CartItems.Count} items.");

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                UserFirstName = user.Firstname,
                UserEmail = user.Email,
                DeliveryAddress = user.Address,
                OrderItems = new List<OrderItem>()
            };

            foreach (var itemId in selectedItems)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == itemId);
                if (cartItem != null)
                {
                    // Log cart item details
                    Console.WriteLine($"Adding item to order: ProductId {cartItem.ProductId}, Quantity {cartItem.Quantity}");

                    // Create the order item
                    var orderItem = new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        UnitPrice = cartItem.UnitPrice
                    };
                    order.OrderItems.Add(orderItem);

                    // Update product quantity
                    var product = await _context.Products.FindAsync(cartItem.ProductId);
                    if (product != null)
                    {
                        product.Quantity -= cartItem.Quantity;
                        _context.Products.Update(product);
                        // Log product update
                        Console.WriteLine($"Updated product quantity for ProductId {product.Id}: New Quantity {product.Quantity}");
                    }

                    // Remove cart item
                    _context.CartItems.Remove(cartItem);
                }
            }

            // Add the order and save changes
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear the cart
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction("OrderConfirmation");
        }
        // CartController.cs

        // Removes a product from the user's cart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null) return NotFound();

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null) return NotFound();

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        // Displays the order confirmation page
        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}
