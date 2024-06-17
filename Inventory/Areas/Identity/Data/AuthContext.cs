using Inventory.Areas.Identity.Data;
using Inventory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Areas.Identity.Data;

public class AuthContext : IdentityDbContext<AuthUser>
{
    public AuthContext(DbContextOptions<AuthContext> options)
        : base(options)
    {
    }


    public DbSet<AuthUser> applicationUsers { get; set; }
    public DbSet<UseActivity>UserActivity {  get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {

        

        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.

        // Add your customizations after calling base.OnModelCreating(builder);
       
    }


}


