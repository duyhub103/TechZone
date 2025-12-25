using MyWeb.Models;

namespace MyWeb.ViewModels
{
    public class ProductDetailViewModel
    {
        public Product MainProduct { get; set; } = null!;
        public List<Product> RelatedProducts { get; set; } = new();

    }
}
