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
    new PredefinedResponse { Id = 1, Question = "What is your name?", Answer = "I am a chatbot!" },
    new PredefinedResponse { Id = 2, Question = "What producs do you have?", Answer = "we have differnet types of products from different catgorise from elcetrical to clothes you can surf around to find more products ." },
    new PredefinedResponse { Id = 3, Question = "How to order?", Answer = "You can add the products and then from the cart yo can select the products and check out to place the order." },
    new PredefinedResponse { Id = 4, Question = "How long for the product to arive to my loacation ?", Answer = "It takes  2-3 working days for the products to be delivered to your location." },
    new PredefinedResponse { Id = 5, Question = "What do you do?", Answer = "I answer questions." },
    new PredefinedResponse { Id = 6, Question = "Thankyou", Answer = "welcome.Fell free to ask any other questions" }
    
);

        // Additional configuration
    }

}
