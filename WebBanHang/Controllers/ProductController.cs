using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.Models;
using WebBanHang.Repositories;

namespace WebBanHang.Controllers
{
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

        // ── Index ──────────────────────────────────────────────
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

        // ── Display ────────────────────────────────────────────
        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // ── Add (GET) ──────────────────────────────────────────
        public IActionResult Add()
        {
            LoadCategories();
            return View();
        }

        // ── Add (POST) ─────────────────────────────────────────
        [HttpPost]
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
                return RedirectToAction("Index");
            }

            LoadCategories();
            return View(product);
        }

        // ── Update (GET) ───────────────────────────────────────
        public IActionResult Update(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            LoadCategories();
            return View(product);
        }

        // ── Update (POST) ──────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> Update(Product product,
            IFormFile? imageUrl, List<IFormFile>? imageUrls)
        {
            if (ModelState.IsValid)
            {
                // Chỉ thay ảnh đại diện nếu người dùng upload file mới
                if (imageUrl != null && imageUrl.Length > 0)
                    product.ImageUrl = await SaveImage(imageUrl);

                // Chỉ thay ảnh phụ nếu người dùng upload file mới
                if (imageUrls != null && imageUrls.Any(f => f.Length > 0))
                {
                    product.ImageUrls = new List<string>();
                    foreach (var file in imageUrls.Where(f => f.Length > 0))
                        product.ImageUrls.Add(await SaveImage(file));
                }

                _productRepository.Update(product);
                return RedirectToAction("Index");
            }

            LoadCategories();
            return View(product);
        }

        // ── Delete (GET) ───────────────────────────────────────
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // ── Delete (POST) ──────────────────────────────────────
        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }

        // ── Helpers ────────────────────────────────────────────
        private void LoadCategories()
        {
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName);
            using var fileStream = new FileStream(savePath, FileMode.Create);
            await image.CopyToAsync(fileStream);
            return "/images/" + image.FileName;
        }
    }
}