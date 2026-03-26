using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.DTOs.Reviews;
using MyWeb.Models;
using MyWeb.Services.Implementations;
using MyWeb.ViewModels;
using System.Globalization;
using System.Security.Claims;

namespace MyWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(string? search, string? type, string? value, int page = 1)
        {
            // Nếu page < 1 thì ép nó về 1 để tránh lỗi SQL Offset
            if (page < 1) page = 1;

            // Giữ lại giá trị filter để truyền lại cho View (dùng cho các link phân trang)
            // khi filter "dell" thì sang trang 2 vẫn giữ filter "dell" không reload all sản phẩm
            ViewData["CurrentSearch"] = search;
            ViewData["CurrentType"] = type;
            ViewData["CurrentValue"] = value;

            var products = await _productService.GetAllProductsAsync(search, type, value, page);
            return View(products);
        }

        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var viewModel = await _productService.GetProductDetailAsync(id, userId);
                return View(viewModel);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMoreReviews(int productId, int page = 2, int pageSize = 10)
        {
            // Gọi Service lấy list review phân trang
            var reviews = await _productService.GetMoreReviewsAsync(productId, page, pageSize);

            if (!reviews.Any()) return NoContent(); // Trả về rỗng nếu hết

            //return PartialView
            return PartialView("Partials/_ReviewList", reviews);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview([FromBody] SubmitReviewRequest req)
        {
            try
            {
                // double check authorize cho an toàn code
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Bạn cần đăng nhập!" });
                }

                //map dto sang model (entity)
                var review = new Review
                {
                    ProductId = req.ProductId,
                    UserId = userId,
                    Rating = req.Rating,
                    Comment = req.Comment,
                };

                var create = await _productService.SubmitReviewAsync(review);

                //trả partial để js append vào list
                return PartialView("Partials/_ReviewItem", create);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        // API cho Live Search (Popup gợi ý sản phẩm)
        [HttpGet]
        public async Task<IActionResult> SearchLive(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return Json(null);

            // Lấy danh sách sản phẩm gợi ý
            var products = await _productService.SearchLiveAsync(query);

            if (!products.Any()) return Json(new { products = new object[] { } });

            // Trả về JSON để JS hiển thị
            var culture = CultureInfo.GetCultureInfo("vi-VN");
            return Json(new
            {
                products = products.Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    price = p.Price,
                    image = p.ImageUrl ?? "/images/default.png"
                })
            });
        }
    }
}
