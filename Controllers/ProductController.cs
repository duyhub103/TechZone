using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.Services.Interfaces;
using MyWeb.ViewModels;
using System.Globalization;
using System.Security.Claims;

namespace MyWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
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
                var viewModel = await _productService.GetProductDetailAsync(id);
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
