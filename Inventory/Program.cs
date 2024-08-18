using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Inventory.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Inventory.HostedServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Inventory.Services;

var builder = WebApplication.CreateBuilder(args);
var openAiApiKey = builder.Configuration["OpenAI:ApiKey"];

// Configuration
var Configuration = builder.Configuration;

// Configure services
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.Expiration = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// Configure database connection
var connectionString = Configuration.GetConnectionString("AuthContextConnection")
                       ?? throw new InvalidOperationException("Connection string 'AuthContextConnection' not found.");

builder.Services.AddDbContext<AuthContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AuthUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthContext>();

// Register GptService with HttpClient
builder.Services.AddHttpClient<GptService>(client =>
{
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");
});
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<GptService>();

builder.Services.Configure<AdminSettings>(Configuration.GetSection("AdminSettings"));
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromHours(24));

builder.Services.AddControllersWithViews();
builder.Services.Configure<EmailSender.EmailSettings>(Configuration.GetSection("emailsettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Register the OrderStatusUpdater hosted service
builder.Services.AddHostedService<OrderStatusUpdater>();

var app = builder.Build();

// Initialize roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        RoleInit.InitializeAsync(services).Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
