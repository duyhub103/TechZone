using Microsoft.AspNetCore.Mvc;

namespace MyWeb.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
