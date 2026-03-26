using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Models;
using MyWeb.Services;
using System.Security.Claims;

namespace MyWeb.Controllers
{
    public class CartController : Controller
    {

        private readonly CartService _cartService;
        

        public CartController(CartService cartService)
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

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var cart = await _cartService.GetCartByUserIdAsync(userId);

                await _cartService.RemoveItemAsync(userId, productId);

                return Json(new { success = true, message = "Đã xóa sản phẩm khỏi giỏ hàng!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _cartService.ClearCartByUserAsync(userId);

                return Json(new { success = true, message = "Đã xóa toàn bộ giỏ hàng!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItemQuantity(int productId, int quantity)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (quantity < 1)
                {
                    return Json(new { success = false, message = "Số lượng tối thiểu là 1." });
                }

                await _cartService.UpdateQuantityAsync(userId, productId, quantity);
                return Json(new { success = true, message = "Cập nhật số lượng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult GetCartItemCount()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = true, itemCount = 0 });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartTask = _cartService.GetCartByUserIdAsync(userId);
            cartTask.Wait();
            var cart = cartTask.Result;

            int itemCount = cart?.Items.Count ?? 0;

            return Json(new { success = true, itemCount = itemCount });
        }

    }
}
