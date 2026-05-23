using Microsoft.AspNetCore.Mvc;
using WebBanHang.Repositories;

namespace WebBanHang.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        // The dependency injection engine automatically provides the repository here
        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            // 1. Fetch data directly
            var products = _productRepository.GetAll();

            // 2. If your repository returns a Task instead, use .Result to force it open for testing:
            // var products = _productRepository.GetAllAsync().Result;

            // 3. Debugging checkpoint: Let's explicitly check if any data exists
            if (products == null || !products.Any())
            {
                ViewBag.Message = "Repository loaded, but your product list is completely empty!";
            }

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}