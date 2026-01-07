using MyWeb.Models;

namespace MyWeb.ViewModels
{
    public class HomeProductListSectionViewModel
    {
        public string Title { get; set; } = "Sản phẩm";
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    }
}
