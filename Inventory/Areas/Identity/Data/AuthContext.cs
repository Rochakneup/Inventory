using Inventory.Areas.Identity.Data;
using Inventory.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class AuthContext : IdentityDbContext<AuthUser>
{
    public AuthContext(DbContextOptions<AuthContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<PredefinedResponse> ChatbotResponses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure the precision and scale for decimal properties
        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,4)"); // Specify precision and scale

        builder.Entity<OrderItem>()
            .Property(oi => oi.UnitPrice)
            .HasColumnType("decimal(18,4)"); // Specify precision and scale

        builder.Entity<CartItem>()
            .Property(ci => ci.UnitPrice)
            .HasColumnType("decimal(18,4)"); // Specify precision and scale

        builder.Entity<CartItem>()
            .Property(ci => ci.ProductImageUrl)
            .HasMaxLength(2048); // Adjust length as needed

        builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

        builder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);

        // Seed data for PredefinedResponse
        builder.Entity<PredefinedResponse>().HasData(

            new PredefinedResponse { Id = 1, Question = "What is your name?", Answer = "I am a chatbot!" },
            new PredefinedResponse { Id = 2, Question = "What products do you have?", Answer = "We have different types of products from different categories, from electrical to clothes. You can surf around to find more products." },
            new PredefinedResponse { Id = 3, Question = "How to order?", Answer = "You can add the products, and then from the cart, you can select the products and check out to place the order." },
            new PredefinedResponse { Id = 4, Question = "How long for the product to arrive at my location?", Answer = "It takes 2-3 working days for the products to be delivered to your location." },
            new PredefinedResponse { Id = 5, Question = "What do you do?", Answer = "I answer questions." },
            new PredefinedResponse { Id = 6, Question = "Thank you", Answer = "You're welcome. Feel free to ask any other questions." }
        );


        // Additional configuration
    }

    // Method to merge data from multiple tables
    public List<MergeddataModel> GetMergedData()
    {
        var mergedData = (from user in Users
                          join order in Orders on user.Email equals order.UserEmail
                          join orderItem in OrderItems on order.Id equals orderItem.OrderId
                          join product in Products on orderItem.ProductId equals product.Id
                          join category in Categories on product.CategoryId equals category.Id
                          join supplier in Suppliers on product.SupplierId equals supplier.Id
                          select new MergeddataModel
                          {
                              UserName = user.UserName,
                              UserEmail = user.Email,
                              CategoryName = category.Name,
                              ProductName = product.Name,
                              Quantity = orderItem.Quantity,
                              UnitPrice = orderItem.UnitPrice,
                              OrderDate = order.OrderDate,
                              SupplierName = supplier.Name,
                              OrderStatus = (OrderStatus)order.Status, // Assuming OrderStatus is an enum
                              DeliveryAddress = order.DeliveryAddress
                          }).ToList();

        return mergedData;
    }

}
