using WebBanHang.Data;
using WebBanHang.Models;

namespace WebBanHang.Repositories
{
	public class EFProductRepository : IProductRepository
	{
		private readonly ApplicationDbContext _context;

		public EFProductRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public IEnumerable<Product> GetAll()
		{
			return _context.Products.ToList();
		}

		public Product GetById(int id)
		{
			// Lấy sản phẩm dựa trên ID
			return _context.Products.FirstOrDefault(p => p.Id == id);
		}

		public void Add(Product product)
		{
			_context.Products.Add(product);
			// Bắt buộc phải có SaveChanges để lưu xuống DB
			_context.SaveChanges();
		}

		public void Update(Product product)
		{
			_context.Products.Update(product);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var product = _context.Products.FirstOrDefault(p => p.Id == id);
			if (product != null)
			{
				_context.Products.Remove(product);
				_context.SaveChanges();
			}
		}
	}
}