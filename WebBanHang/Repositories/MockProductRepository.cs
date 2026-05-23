using WebBanHang.Models;

namespace WebBanHang.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;
        public MockProductRepository()
        {
            // Tạo một số dữ liệu mẫu
            _products = new List<Product>
                {
                    // Laptops (CategoryId = 1)
                    new Product
                        {
                            Id = 1,
                            Name = "MacBook Pro M3",
                            Price = 29000000,
                            Description = "Apple Silicon, 16GB RAM, 512GB SSD",
                            CategoryId = 1,
                            ImageUrl = "/images/MacBook Pro M3.jpg" // Ảnh đại diện chính xác từ thư mục
                        },
                        new Product
                        {
                            Id = 2,
                            Name = "Dell XPS 15",
                            Price = 15000000,
                            Description = "Intel i7, 4K Display, Great for creators",
                            CategoryId = 1,
                            ImageUrl = "/images/Dell XPS 15.jpg", // Ảnh chính
                            ImageUrls = new List<string>
                            {
                                "/images/Dell XPS 15 2.jpg", // Ảnh phụ 1
                                "/images/Dell XPS 15 3.jpg"  // Ảnh phụ 2
                            }
                        },
                        new Product
                        {
                            Id = 3,
                            Name = "ASUS ROG Zephyrus",
                            Price = 18000000,
                            Description = "High-end gaming laptop with RTX 4070",
                            CategoryId = 1,
                            ImageUrl = "/images/ASUS ROG Zephyrus.jpg" // Sử dụng tạm ảnh có sẵn nếu chưa có ảnh riêng
                        },

                        // Desktops (CategoryId = 2)
                        new Product { 
                            Id = 4, 
                            Name = "iMac 24\"", 
                            Price = 13000000, 
                            Description = "All-in-one desktop with M3 chip", 
                            CategoryId = 2, 
                            ImageUrl = "/images/iMac 24.jpg"
                        },
                        new Product { 
                            Id = 5, 
                            Name = "Alienware Aurora R16", 
                            Price = 22000000, 
                            Description = "Liquid-cooled ultimate gaming desktop PC", 
                            CategoryId = 2, 
                            ImageUrl = "/images/Alienware Aurora R16.jpg"
                        },

                        // Accessories (CategoryId = 3)
                        new Product { 
                            Id = 6, 
                            Name = "Logitech MX Master 3S",
                            Price = 1000000,
                            Description = "Ergonomic wireless mouse for productivity",
                            CategoryId = 3, 
                            ImageUrl = "/images/Logitech MX Master 3S.jpg"
                        },
                        new Product { 
                            Id = 7, 
                            Name = "Mechanical Keyboard GMMK 2",
                            Price = 12000000,
                            Description = "Hot-swappable RGB mechanical keyboard",
                            CategoryId = 3, 
                            ImageUrl = "/images/Mechanical Keyboard GMMK 2.jpg"
                        },

                        // Smartphones (CategoryId = 4)
                        new Product { 
                            Id = 8, 
                            Name = "iPhone 15 Pro", 
                            Price = 10000000,
                            Description = "Titanium design, Action button, A17 Pro chip", 
                            CategoryId = 4, 
                            ImageUrl = "/images/iPhone 15 Pro.jpg"
                        },
                        new Product { 
                            Id = 9, 
                            Name = "Samsung Galaxy S24 Ultra", 
                            Price = 12000000, 
                            Description = "Built-in S Pen, Galaxy AI, 200MP Camera",
                            CategoryId = 4, 
                            ImageUrl = "/images/Samsung Galaxy S24 Ultra.jpg"
                        }
                };
        }
        public IEnumerable<Product> GetAll()
        {
            return _products;
        }
        public Product GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
        public void Add(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
        }
        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }
        }
        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}
