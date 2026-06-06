using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebBanHang.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrls = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Laptop" },
                    { 2, "Desktop" },
                    { 3, "Accessories" },
                    { 4, "Smartphones" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "ImageUrls", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Apple Silicon, 16GB RAM, 512GB SSD", "/images/MacBook Pro M3.jpg", null, "MacBook Pro M3", 29000000m },
                    { 2, 1, "Intel i7, 4K Display, Great for creators", "/images/Dell XPS 15.jpg", "[\"/images/Dell XPS 15 2.jpg\",\"/images/Dell XPS 15 3.jpg\"]", "Dell XPS 15", 15000000m },
                    { 3, 1, "High-end gaming laptop with RTX 4070", "/images/ASUS ROG Zephyrus.jpg", null, "ASUS ROG Zephyrus", 18000000m },
                    { 4, 2, "All-in-one desktop with M3 chip", "/images/iMac 24.jpg", null, "iMac 24\"", 13000000m },
                    { 5, 2, "Liquid-cooled ultimate gaming desktop PC", "/images/Alienware Aurora R16.jpg", null, "Alienware Aurora R16", 22000000m },
                    { 6, 3, "Ergonomic wireless mouse for productivity", "/images/Logitech MX Master 3S.jpg", null, "Logitech MX Master 3S", 1000000m },
                    { 7, 3, "Hot-swappable RGB mechanical keyboard", "/images/Mechanical Keyboard GMMK 2.jpg", null, "Mechanical Keyboard GMMK 2", 12000000m },
                    { 8, 4, "Titanium design, Action button, A17 Pro chip", "/images/iPhone 15 Pro.jpg", null, "iPhone 15 Pro", 10000000m },
                    { 9, 4, "Built-in S Pen, Galaxy AI, 200MP Camera", "/images/Samsung Galaxy S24 Ultra.jpg", null, "Samsung Galaxy S24 Ultra", 12000000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
