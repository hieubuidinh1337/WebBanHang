using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // ✅ Thêm thư viện này cho SelectList
using WebBanHang.Repositories;

namespace WebBanHang.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository; // ✅ Thêm repository này

        // Cập nhật constructor để inject ICategoryRepository
        public HomeController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index(string searchTerm, int? categoryId)
        {
            var products = _productRepository.GetAll();

            // 1. Lọc theo từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }

            // 2. Lọc theo danh mục
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            // 3. Lấy dữ liệu danh mục cho thanh tìm kiếm
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CurrentCategoryId = categoryId;

            // 4. Debugging checkpoint
            if (products == null || !products.Any())
            {
                ViewBag.Message = "Không tìm thấy sản phẩm nào phù hợp với yêu cầu của bạn!";
            }

            return View(products);
        }

        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}