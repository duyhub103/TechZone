using MyWeb.Models;

namespace MyWeb.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> FeaturedProducts { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Product> LatestProducts { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Product> BestSellingProducts { get; set; } = Enumerable.Empty<Product>();

        public IEnumerable<Banner> MainSliders { get; set; } = Enumerable.Empty<Banner>();
        public Banner? PromoBanner { get; set; } // cho an toàn null

        public List<HomeCategory> PopularCategories { get; set; } = new();
    }
}
