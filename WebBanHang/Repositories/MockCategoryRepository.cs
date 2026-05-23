using WebBanHang.Models;

namespace WebBanHang.Repositories
{
    public class MockCategoryRepository : ICategoryRepository
    {
        private List<Category> _categoryList;
        public MockCategoryRepository()
        {
            _categoryList = new List<Category>
            {
                new Category { Id = 1, Name = "Laptop" },
                new Category { Id = 2, Name = "Desktop" },
                new Category { Id = 3, Name = "Accessories" },
                new Category { Id = 4, Name = "Smartphones" }
            };
        }
        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryList;
        }
    }
}
