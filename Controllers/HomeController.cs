using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.Models;
using MyWeb.Services.Implementations;
using MyWeb.ViewModels;
using System.Diagnostics;

namespace MyWeb.Controllers
{
	public class HomeController : Controller
    { 
        private readonly HomeService _homeService;

		public HomeController(HomeService homeService)
		{
            _homeService = homeService;
		}

		public async Task<IActionResult> Index()
		{
            var viewModel = await _homeService.GetHomeViewModelAsync();

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
