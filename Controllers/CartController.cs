using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Models;
using MyWeb.Services.Interfaces;
using System.Security.Claims;

namespace MyWeb.Controllers
{
    public class CartController : Controller
    {

        private readonly ICartService _cartService;
        

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        //public IActionResult Index()
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var viewModel = _cartService.GetCartByUserId(userId);

        //    return View(viewModel);
        //}


        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var vm = await _cartService.GetCartByUserIdAsync(userId);
            return View(vm);
        }


        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity, string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account",
                    new { returnUrl });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _cartService.AddToCartAsync(userId, productId, quantity);

            return Redirect(returnUrl ?? "/Cart");
        }
    }
}
