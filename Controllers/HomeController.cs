using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.Models;
using MyWeb.ViewModels;
using System.Diagnostics;

namespace MyWeb.Controllers
{
	public class HomeController : Controller
    { 
        private readonly TechZoneDbContext _context;

		public HomeController(TechZoneDbContext context)
		{
            _context = context;
		}

		public IActionResult Index()
		{
            var featuredProducts = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Attributes)
                .Where(p => p.IsActive && p.IsFeatured)
                .OrderByDescending(p => p.Id)
                .Take(4)
                .ToList();


            var sliders = _context.Banners
                .Where(b => b.IsActive && b.Position == BannerPosition.MainSlider)
                .OrderBy(b => b.DisplayOrder)
                .ToList();

            var promo = _context.Banners
                .Where(b => b.IsActive && b.Position == BannerPosition.PromoSection)
                .OrderBy(b => b.DisplayOrder)
                .FirstOrDefault();

            var viewModel = new HomeViewModel
            {
                FeaturedProducts = featuredProducts,
                MainSliders = sliders,
                PromoBanner = promo
            };

            return View(viewModel);
		}


        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        // Trang Chính sách
        public IActionResult Policies()
        {
            return View();
        }

        // Trang 404
        [Route("Home/Error404")]
        public IActionResult Error404()
        {
            return View();
        }


    }
}
