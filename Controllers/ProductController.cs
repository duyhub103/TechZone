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

        public IActionResult Detail(int id)
        {
            try
            {
                var viewModel = _productService.GetProductDetail(id);
                return View(viewModel);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }       
    }
}
