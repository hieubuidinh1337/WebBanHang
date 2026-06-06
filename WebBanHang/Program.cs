using Microsoft.EntityFrameworkCore;
using WebBanHang.Data;
using WebBanHang.Repositories;
using Microsoft.AspNetCore.Identity;
using WebBanHang.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
builder.Services.AddScoped<IProductRepository, EFProductRepository>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

if (false)
{
    app.UseExceptionHandler("/Home/Error");
}

    app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// ✅ Route Area Admin — phải đứng TRƯỚC route default
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");

// ✅ Route mặc định → HomeController (public, không cần login)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();