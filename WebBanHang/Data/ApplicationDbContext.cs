using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebBanHang.Models;

namespace WebBanHang.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Xử lý kiểu List<string> cho SQL Server bằng cách ép sang JSON string
			modelBuilder.Entity<Product>()
				.Property(p => p.ImageUrls)
				.HasConversion(
					v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
					v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
				);

			// 1. Đẩy dữ liệu Category
			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Laptop" },
				new Category { Id = 2, Name = "Desktop" },
				new Category { Id = 3, Name = "Accessories" },
				new Category { Id = 4, Name = "Smartphones" }
			);

			// 2. Đẩy dữ liệu Product
			modelBuilder.Entity<Product>().HasData(
				new Product { Id = 1, Name = "MacBook Pro M3", Price = 29000000, Description = "Apple Silicon, 16GB RAM, 512GB SSD", CategoryId = 1, ImageUrl = "/images/MacBook Pro M3.jpg" },
				new Product { Id = 2, Name = "Dell XPS 15", Price = 15000000, Description = "Intel i7, 4K Display, Great for creators", CategoryId = 1, ImageUrl = "/images/Dell XPS 15.jpg", ImageUrls = new List<string> { "/images/Dell XPS 15 2.jpg", "/images/Dell XPS 15 3.jpg" } },
				new Product { Id = 3, Name = "ASUS ROG Zephyrus", Price = 18000000, Description = "High-end gaming laptop with RTX 4070", CategoryId = 1, ImageUrl = "/images/ASUS ROG Zephyrus.jpg" },
				new Product { Id = 4, Name = "iMac 24\"", Price = 13000000, Description = "All-in-one desktop with M3 chip", CategoryId = 2, ImageUrl = "/images/iMac 24.jpg" },
				new Product { Id = 5, Name = "Alienware Aurora R16", Price = 22000000, Description = "Liquid-cooled ultimate gaming desktop PC", CategoryId = 2, ImageUrl = "/images/Alienware Aurora R16.jpg" },
				new Product { Id = 6, Name = "Logitech MX Master 3S", Price = 1000000, Description = "Ergonomic wireless mouse for productivity", CategoryId = 3, ImageUrl = "/images/Logitech MX Master 3S.jpg" },
				new Product { Id = 7, Name = "Mechanical Keyboard GMMK 2", Price = 12000000, Description = "Hot-swappable RGB mechanical keyboard", CategoryId = 3, ImageUrl = "/images/Mechanical Keyboard GMMK 2.jpg" },
				new Product { Id = 8, Name = "iPhone 15 Pro", Price = 10000000, Description = "Titanium design, Action button, A17 Pro chip", CategoryId = 4, ImageUrl = "/images/iPhone 15 Pro.jpg" },
				new Product { Id = 9, Name = "Samsung Galaxy S24 Ultra", Price = 12000000, Description = "Built-in S Pen, Galaxy AI, 200MP Camera", CategoryId = 4, ImageUrl = "/images/Samsung Galaxy S24 Ultra.jpg" }
			);
		}
	}
}