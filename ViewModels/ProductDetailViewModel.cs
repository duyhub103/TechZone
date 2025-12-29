using MyWeb.Models;

namespace MyWeb.ViewModels
{
    public class ProductDetailViewModel
    {
        public Product MainProduct { get; set; } = null!;
        public List<Product> RelatedProducts { get; set; } = new();


        public List<Review> Reviews { get; set; } = new();
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }

        // Mảng chứa số lượng đánh giá cho từng sao (Index 0 = 1 sao, Index 4 = 5 sao)
        public int[] StarCounts { get; set; } = new int[5];
    }
}
