using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Inventory.Areas.Identity.Data;
using Inventory.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AuthContext _context;

        public CartController(AuthContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                var cart = GetCart();
                var cartItem = cart.CartItems.FirstOrDefault(c => c.ProductId == productId);

                if (cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    cart.CartItems.Add(new CartItem
                    {
                        ProductId = productId,
                        ProductName = product.Name,
                        Quantity = 1,
                        UnitPrice = product.Price
                    });
                }

                SaveCart(cart);
            }

            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var cartItem = cart.CartItems.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem != null)
            {
                cart.CartItems.Remove(cartItem);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        private Cart GetCart()
        {
            var userId = User.Identity.Name;
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    CartItems = new List<CartItem>()
                };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            // Populate ProductName for each cart item
            foreach (var item in cart.CartItems)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product != null)
                {
                    item.ProductName = product.Name;
                }
            }

            return cart;
        }

        private void SaveCart(Cart cart)
        {
            _context.Update(cart);
            _context.SaveChanges();
        }

        public IActionResult Checkout()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult PlaceOrder()
        {
            var cart = GetCart();
            if (cart.CartItems.Count == 0)
            {
                return RedirectToAction("Index");
            }

            var order = new Order
            {
                UserId = cart.UserId,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                OrderItems = cart.CartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    UnitPrice = c.UnitPrice
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.Carts.Remove(cart);
            _context.SaveChanges();

            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }

        public IActionResult OrderConfirmation(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == orderId);

            return View(order);
        }
    }
}
