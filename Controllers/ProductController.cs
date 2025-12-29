using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.Services.Interfaces;
using MyWeb.ViewModels;
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

        public IActionResult Index(string? type, string? value)
        {
            var products = _productService.GetAllProducts(type, value);
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

            // Trả về PartialView, không phải JSON, để render HTML luôn
            // Lưu ý: Tạo view tên _ReviewItemList nếu muốn loop ở view, 
            // hoặc trả về từng item (ở đây tôi loop trong Action trả về string HTML hoặc return PartialView loop).

            // Cách đơn giản nhất: return PartialView
            return PartialView("Partials/_ReviewList", reviews);
        }
    }
}
