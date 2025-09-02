using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScottMo.Web.Data;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// ---------- DataProtection: persist keys (prevents antiforgery failures after restarts)
var keysPath = Path.Combine(builder.Environment.ContentRootPath, "keys");
Directory.CreateDirectory(keysPath);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
    .SetApplicationName("ScottMo.Web");

// ---------- Connection string: read from env var injected by GitHub Actions
// Primary: ConnectionStrings__Default
// Fallbacks: Default__ConnectionString or appsettings.json "ConnectionStrings:Default"
var conn =
    builder.Configuration.GetConnectionString("Default")
    ?? builder.Configuration["ConnectionStrings__Default"]
    ?? builder.Configuration["Default__ConnectionString"];

if (string.IsNullOrWhiteSpace(conn))
{
    throw new InvalidOperationException("No connection string found. Ensure the env var ConnectionStrings__Default is set.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(conn));

// ---------- Identity
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        // optional: relax password for testing, adjust as needed
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

// ---------- Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();          // fine even on temp HTTP URL; will no-op if no https port
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// quick health ping
app.MapGet("/api/hello", () => Results.Ok(new { ok = true, time = DateTime.UtcNow }));

app.Run();
