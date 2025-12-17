using Microsoft.AspNetCore.Mvc;
using MyWeb.Models;
using System.Diagnostics;

namespace MyWeb.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
            ViewData["Title"] = "Home";
            return View();
		}

        // Trang Gi?i thi?u
        public IActionResult About()
        {
            return View();
        }

        // Trang Liên h?
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
