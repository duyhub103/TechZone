using MyWeb.Models;
using MyWeb.Repositories.Interfaces;
using MyWeb.Services.Interfaces;
using MyWeb.ViewModels;
using Microsoft.EntityFrameworkCore; // Cần cái này nếu dùng Async methods của EF

namespace MyWeb.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IReviewRepository _reviewRepo;

        public ProductService(IProductRepository productRepo, IReviewRepository reviewRepo)
        {
            _productRepo = productRepo;
            _reviewRepo = reviewRepo;
        }

        public IEnumerable<Product> GetAllProducts(string? type = null, string? value = null)
        {
            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(value))
            {
                return _productRepo.GetByFilter(type, value);
            }
            return _productRepo.GetAllActive();
        }

        // --- HÀM NÀY ĐÃ ĐƯỢC NÂNG CẤP ---
        public async Task<ProductDetailViewModel> GetProductDetailAsync(int id)
        {
            // 1. Lấy thông tin sản phẩm (Vẫn giữ sync nếu repo của bạn là sync, không sao cả)
            var mainProduct = _productRepo.GetById(id);
            if (mainProduct == null)
                throw new Exception("Product not found");

            // 2. Lấy sản phẩm liên quan
            var relatedProducts = _productRepo.GetRelatedProducts(
                mainProduct.CategoryId,
                id,
                4
            );

            // 3. Lấy TẤT CẢ review để tính toán thống kê (Gọi hàm Async từ Repo)
            var allReviews = await _reviewRepo.GetAllReviewsByProductIdAsync(id);

            // 4. Tính toán số liệu thống kê
            int totalReviews = allReviews.Count;
            double averageRating = 0;
            int[] starCounts = new int[5]; // Mảng chứa số lượng từng sao (1-5)

            if (totalReviews > 0)
            {
                // Tính điểm trung bình
                averageRating = allReviews.Average(r => r.Rating);

                // Đếm số lượng từng loại sao
                starCounts[0] = allReviews.Count(r => r.Rating == 1); // 1 sao
                starCounts[1] = allReviews.Count(r => r.Rating == 2); // 2 sao
                starCounts[2] = allReviews.Count(r => r.Rating == 3); // 3 sao
                starCounts[3] = allReviews.Count(r => r.Rating == 4); // 4 sao
                starCounts[4] = allReviews.Count(r => r.Rating == 5); // 5 sao
            }

            // 5. Lấy 10 review mới nhất để hiển thị ban đầu
            var initialReviews = allReviews
                                .OrderByDescending(r => r.CreatedAt)
                                .Take(10)
                                .ToList();

            // 6. Đóng gói vào ViewModel
            return new ProductDetailViewModel
            {
                MainProduct = mainProduct,
                RelatedProducts = relatedProducts.ToList(),

                // Dữ liệu Review
                Reviews = initialReviews,
                TotalReviews = totalReviews,
                AverageRating = averageRating,
                StarCounts = starCounts
            };
        }

        public async Task<List<Review>> GetMoreReviewsAsync(int productId, int page, int pageSize)
        {
            return await _reviewRepo.GetReviewsByProductAsync(productId, page, pageSize);
        }
    }
}