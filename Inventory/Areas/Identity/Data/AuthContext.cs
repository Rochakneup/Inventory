using Inventory.Areas.Identity.Data;
using Inventory.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

        builder.Entity<OrderItem>()
            .Property(oi => oi.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        builder.Entity<CartItem>()
            .Property(ci => ci.UnitPrice)
            .HasColumnType("decimal(18,2)");

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

        builder.Entity<PredefinedResponse>().HasData(
     new PredefinedResponse { Id = 1, Question = "Can you help me find a product?", Answer = "I'm unable to help with finding specific categories, but you can browse through our website or use the search feature to find the products you're looking for." },
     new PredefinedResponse { Id = 2, Question = "What are your shipping options?", Answer = "We offer various shipping options including standard, expedited, and express delivery. You can choose the one that best fits your needs during checkout." },
     new PredefinedResponse { Id = 3, Question = "How can I track my order?", Answer = "To track your order, please use the tracking number sent to you via email. You can enter this number on our website's order tracking page to see the current status of your shipment." },
     new PredefinedResponse { Id = 4, Question = "What is your return policy?", Answer = "We offer a 30-day return policy for most items. If you're not satisfied with your purchase, you can return it within 30 days for a full refund or exchange." },
     new PredefinedResponse { Id = 5, Question = "Do you offer discounts or promotions?", Answer = "Yes, we frequently offer discounts and promotions. Be sure to check our website's promotions page or subscribe to our newsletter for the latest deals." },
     new PredefinedResponse { Id = 6, Question = "How can I contact customer support?", Answer = "You can contact our customer support team via email at support@example.com. We will be happy to assist you with any questions or concerns." },
     new PredefinedResponse { Id = 7, Question = "Thankyou", Answer = "You're Welcome!" }



);

        // Additional configuration
    }

}
