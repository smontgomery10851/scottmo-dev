using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScottMo.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Prefer environment variables (for secrets) over appsettings.json
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? Environment.GetEnvironmentVariable("Default__ConnectionString")
    ?? throw new InvalidOperationException("Connection string 'Default' not found.");

// Services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

builder.Services.AddRazorPages();
builder.Services.AddControllers(); // Web API

var app = builder.Build();

// DO NOT auto-migrate to avoid changing existing schema on shared host.
// If you add features later, run migrations locally and publish scripts manually.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
