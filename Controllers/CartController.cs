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

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var vm = await _cartService.GetCartByUserIdAsync(userId);
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            // check login
            if (!User.Identity.IsAuthenticated)
            {
                
                return Json(new
                {
                    message = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng.",
                    success = false,
                    requireLogin = true,
                    loginUrl = Url.Action("Login", "Account", new { returnUrl = Request.Headers["Referer"].ToString() })
                });
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _cartService.AddToCartAsync(userId, productId, quantity);

                // lấy giỏ hàng mới nhất
                var updateCart = await _cartService.GetCartByUserIdAsync(userId);

                // tổng mặt hàng trong giỏ
                int totalItem = updateCart.Items.Count;


                return Json(new { 
                    success = true, 
                    message = "Đã thêm vào giỏ hàng thành công!",
                    newTotalItem = totalItem
                });
            }
            catch (Exception ex)
            {
                return Json(new {
                    success = false,
                    message = "Đã có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng: " + ex.Message
                });
            }
        }
    }
}
