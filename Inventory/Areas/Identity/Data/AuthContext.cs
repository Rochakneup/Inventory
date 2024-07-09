using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Inventory.Areas.Identity.Data;
using Inventory.Models;

namespace Inventory.Areas.Identity.Data
{
    public class AuthContext : IdentityDbContext<AuthUser>
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

        public DbSet<AuthUser> applicationUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; } // Add the Category DbSet

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure decimal precision and scale for UnitPrice
            builder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)");
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Configure Product-Category relationship
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Add other configurations as needed
        }
    }
}
