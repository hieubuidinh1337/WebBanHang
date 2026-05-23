using Microsoft.AspNetCore.Mvc;
using WebBanHang.Models;
using WebBanHang.Repositories;
using WebBanHang.Extensions; // Gọi thư viện Extension ta vừa viết ở trên

namespace WebBanHang.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private const string CART_KEY = "MyCart";

        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // 1. Hiển thị Giỏ hàng
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
            return View(cart);
        }

        // 2. Thêm sản phẩm vào Giỏ
        public IActionResult AddToCart(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }

            // Lấy giỏ hàng hiện tại (nếu chưa có thì tạo list mới)
            var cart = HttpContext.Session.GetJson<List<CartItem>>(CART_KEY) ?? new List<CartItem>();

            // Kiểm tra xem sản phẩm này đã có trong giỏ chưa
            var cartItem = cart.FirstOrDefault(c => c.Product.Id == id);

            if (cartItem != null)
            {
                cartItem.Quantity += 1; // Nếu có rồi thì tăng số lượng
            }
            else
            {
                cart.Add(new CartItem { Product = product, Quantity = 1 }); // Nếu chưa có thì thêm mới
            }

            // Lưu lại vào Session
            HttpContext.Session.SetJson(CART_KEY, cart);

            return RedirectToAction("Index");
        }

        // 3. Xóa sản phẩm khỏi giỏ
        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>(CART_KEY);
            if (cart != null)
            {
                cart.RemoveAll(p => p.Product.Id == id);
                HttpContext.Session.SetJson(CART_KEY, cart);
            }
            return RedirectToAction("Index");
        }

        // 4. Cập nhật số lượng (+ / -)
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>(CART_KEY);
            if (cart != null)
            {
                var cartItem = cart.FirstOrDefault(c => c.Product.Id == id);
                if (cartItem != null)
                {
                    if (quantity > 0)
                        cartItem.Quantity = quantity;
                    else
                        cart.Remove(cartItem); // Nếu khách giảm số lượng về 0 thì tự động xóa
                }
                HttpContext.Session.SetJson(CART_KEY, cart);
            }
            return RedirectToAction("Index");
        }

        // 5. Hiển thị trang Thanh toán
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
            if (!cart.Any())
            {
                return RedirectToAction("Index"); // Nếu giỏ trống thì quay lại trang giỏ hàng
            }
            return View(cart);
        }

        // 6. Xử lý khi nhấn "Xác nhận đặt hàng"
        [HttpPost]
        public IActionResult PlaceOrder(string FullName, string Phone, string Address, string Notes, string PaymentMethod)
        {
            // TODO: Ở bước sau, bạn có thể gọi OrderRepository để lưu thông tin người mua xuống Database

            // Thanh toán xong thì xóa sạch Giỏ hàng
            HttpContext.Session.Remove(CART_KEY);

            // Chuyển hướng người dùng về trang chủ
            return RedirectToAction("Index", "Product");
        }
    }
}