using WebBanHang.Data;
using WebBanHang.Models;

namespace WebBanHang.Repositories
{
	public class EFCategoryRepository : ICategoryRepository
	{
		private readonly ApplicationDbContext _context;

		public EFCategoryRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public IEnumerable<Category> GetAllCategories()
		{
			return _context.Categories.ToList();
		}
	}
}