using Microsoft.AspNetCore.Mvc;

namespace MyWeb.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Cart/Checkout
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
