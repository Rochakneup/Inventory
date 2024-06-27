using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Inventory.Areas.Identity.Data; // Adjust namespace as per your project structure
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure decimal precision and scale for UnitPrice
            builder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)"); // Adjust precision and scale as per your requirements

            // Add other configurations as needed

            // Example: To configure Product table
            // builder.Entity<Product>()
            //    .Property(p => p.Price)
            //    .HasColumnType("decimal(18,2)");

            // Example: To configure Supplier table
            // builder.Entity<Supplier>()
            //    .Property(s => s.Quantity)
            //    .HasColumnType("int");

            // Example: To configure other entities, follow similar patterns

        }
    }
}
