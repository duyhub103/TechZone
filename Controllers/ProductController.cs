using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.ViewModels;

namespace MyWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly TechZoneDbContext _context;

        public ProductController(TechZoneDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.Attributes)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .ToList();
            return View(products);
        }

        public IActionResult Detail(int id)
        {

            //lấy sản phầm chính
            var mainProduct = _context.Products
                .Include(p => p.Attributes)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.Id == id && p.IsActive);

            if(mainProduct == null)
            {
                return NotFound();
            }

            //lấy 4 sản phẩm liên quan cùng danh mục bao gồm attribute
            var relatedProducts = _context.Products
                .Where(p => p.CategoryId == mainProduct.CategoryId && p.Id != id && p.IsActive)
                .Include(p => p.Attributes)
                .OrderByDescending(p => p.IsFeatured)
                .Take(4)
                .ToList();
            // gán vào VM
            var viewModel = new ProductDetailViewModel
            {
                MainProduct = mainProduct,
                RelatedProducts = relatedProducts
            };

            return View(viewModel);
        }
    }
}
