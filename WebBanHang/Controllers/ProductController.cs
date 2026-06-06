using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.Models;
using WebBanHang.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace WebBanHang.Controllers
{
    [Area("Admin")]                        // ✅ Giữ nguyên
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // ... (Các phần ở trên giữ nguyên) ...

        public IActionResult Index(string searchTerm, int? categoryId)
        {
            // Lấy toàn bộ sản phẩm từ Repository
            var products = _productRepository.GetAll();

            // Lọc theo tên sản phẩm (Không phân biệt chữ hoa, chữ thường)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }

            // Lọc theo danh mục
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            // Lấy danh sách Category để đưa vào Dropdown List
            var categories = _categoryRepository.GetAllCategories();

            // Gửi dữ liệu qua ViewBag để hiển thị trên View
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CurrentCategoryId = categoryId;

            return View(products);
        }

        // ... (Các phần ở dưới giữ nguyên) ...

        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        public IActionResult Add()
        {
            LoadCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product,
            IFormFile imageUrl, List<IFormFile> imageUrls)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                    product.ImageUrl = await SaveImage(imageUrl);

                if (imageUrls != null && imageUrls.Any())
                {
                    product.ImageUrls = new List<string>();
                    foreach (var file in imageUrls)
                        product.ImageUrls.Add(await SaveImage(file));
                }

                _productRepository.Add(product);
                // ✅ RedirectToAction phải truyền area
                return RedirectToAction("Index", new { area = "Admin" });
            }

            LoadCategories();
            return View(product);
        }

        public IActionResult Update(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            LoadCategories();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Product product,
            IFormFile? imageUrl, List<IFormFile>? imageUrls)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null && imageUrl.Length > 0)
                    product.ImageUrl = await SaveImage(imageUrl);

                if (imageUrls != null && imageUrls.Any(f => f.Length > 0))
                {
                    product.ImageUrls = new List<string>();
                    foreach (var file in imageUrls.Where(f => f.Length > 0))
                        product.ImageUrls.Add(await SaveImage(file));
                }

                _productRepository.Update(product);
                // ✅ RedirectToAction phải truyền area
                return RedirectToAction("Index", new { area = "Admin" });
            }

            LoadCategories();
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            // ✅ RedirectToAction phải truyền area
            return RedirectToAction("Index", new { area = "Admin" });
        }

        private void LoadCategories()
        {
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var savePath = Path.Combine("wwwroot/images", uniqueFileName);
            Directory.CreateDirectory(Path.Combine("wwwroot", "images"));
            using var fileStream = new FileStream(savePath, FileMode.Create);
            await image.CopyToAsync(fileStream);
            return "/images/" + uniqueFileName;
        }
    }
}