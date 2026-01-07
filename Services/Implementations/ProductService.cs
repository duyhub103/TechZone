using MyWeb.Models;
using MyWeb.Repositories.Interfaces;
using MyWeb.ViewModels;
using Microsoft.EntityFrameworkCore; // Cần cái này nếu dùng Async methods của EF

namespace MyWeb.Services.Implementations
{
    public class ProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IReviewRepository _reviewRepo;

        public ProductService(IProductRepository productRepo, IReviewRepository reviewRepo)
        {
            _productRepo = productRepo;
            _reviewRepo = reviewRepo;
        }

        public async Task<PaginatedList<Product>> GetAllProductsAsync(string? keyword, string? type, string? value, int pageIndex = 1)
        {
            int pageSize = 4; // Số lượng sản phẩm/trang
            return await _productRepo.GetProductsAsync(keyword, type, value, pageIndex, pageSize);
        }

        public async Task<IEnumerable<Product>> SearchLiveAsync(string keyword)
        {
            return await _productRepo.SearchLiveAsync(keyword);
        }

        public async Task<ProductDetailViewModel> GetProductDetailAsync(int id)
        {
            //  Lấy thông tin sản phẩm
            var mainProduct = await _productRepo.GetByIdAsync(id);
            if (mainProduct == null)
                throw new Exception("Product not found");

            //  Lấy sản phẩm liên quan
            var relatedProducts = await _productRepo.GetRelatedProductsAsync(
                mainProduct.CategoryId,
                id,
                4
            );

            //  Lấy TẤT CẢ review để tính toán thống kê
            var allReviews = await _reviewRepo.GetAllReviewsByProductIdAsync(id);

            //  Tính toán số liệu thống kê
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
                starCounts[2] = allReviews.Count(r => r.Rating == 3); 
                starCounts[3] = allReviews.Count(r => r.Rating == 4); 
                starCounts[4] = allReviews.Count(r => r.Rating == 5); 
            }

            //  Lấy 10 review mới nhất để hiển thị ban đầu
            var initialReviews = allReviews
                                .OrderByDescending(r => r.CreatedAt)
                                .Take(10)
                                .ToList();

            //  Đóng gói vào ViewModel
            return new ProductDetailViewModel
            {
                MainProduct = mainProduct,
                RelatedProducts = relatedProducts.ToList(),

                // Review
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