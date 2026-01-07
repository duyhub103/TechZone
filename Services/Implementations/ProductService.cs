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
        private readonly IOrderRepository _orderRepo;

        public ProductService(
            IProductRepository productRepo,
            IReviewRepository reviewRepo,
            IOrderRepository orderRepo)
        {
            _productRepo = productRepo;
            _reviewRepo = reviewRepo;
            _orderRepo = orderRepo;
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
            var mainProduct = await _productRepo.GetByIdAsync(id);
            if (mainProduct == null)
                throw new Exception("Product not found");

            var relatedProducts = await _productRepo.GetRelatedProductsAsync(mainProduct.CategoryId, id, 4);
            var allReviews = await _reviewRepo.GetAllReviewsByProductIdAsync(id);

            int totalReviews = allReviews.Count;
            double averageRating = 0;
            int[] starCounts = new int[5];

            if (totalReviews > 0)
            {
                averageRating = allReviews.Average(r => r.Rating);
                starCounts[0] = allReviews.Count(r => r.Rating == 1);
                starCounts[1] = allReviews.Count(r => r.Rating == 2);
                starCounts[2] = allReviews.Count(r => r.Rating == 3);
                starCounts[3] = allReviews.Count(r => r.Rating == 4);
                starCounts[4] = allReviews.Count(r => r.Rating == 5);
            }

            var initialReviews = allReviews
                .OrderByDescending(r => r.CreatedAt)
                .Take(10)
                .ToList();
           
            return new ProductDetailViewModel
            {
                MainProduct = mainProduct,
                RelatedProducts = relatedProducts.ToList(),

                Reviews = initialReviews,
                TotalReviews = totalReviews,
                AverageRating = averageRating,
                StarCounts = starCounts
            };
        }



        public async Task<ProductDetailViewModel> GetProductDetailAsync(int id, string? userId)
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

            //  Lấy tất cả review để tính toán thống kê
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

            bool canReview = false;
            if (!string.IsNullOrEmpty(userId))
            {
                var hasPurchased = await _orderRepo.HasUserPurchasedProductAsync(userId, id);
                if (hasPurchased)
                {
                    var hasReviewed = await _reviewRepo.HasUserReviewedProductAsync(id, userId);
                    canReview = !hasReviewed;
                }
            }


            //  Đóng gói vào ViewModel
            return new ProductDetailViewModel
            {
                MainProduct = mainProduct,
                RelatedProducts = relatedProducts.ToList(),

                // Review
                Reviews = initialReviews,
                TotalReviews = totalReviews,
                AverageRating = averageRating,
                StarCounts = starCounts,
                CanReview = canReview
            };
        }

        public async Task<List<Review>> GetMoreReviewsAsync(int productId, int page, int pageSize)
        {
            return await _reviewRepo.GetReviewsByProductAsync(productId, page, pageSize);
        }

        //submit review (server validate lại request để không bị bypass)
        public async Task<Review> SubmitReviewAsync(Review review)
        {
            if (review == null)
                throw new ArgumentNullException(nameof(review), "Đánh giá không được null.");

            if (review.ProductId <= 0)
                throw new ArgumentException("Sản phẩm không hợp lệ.", nameof(review.ProductId));

            if (string.IsNullOrWhiteSpace(review.UserId))
                throw new ArgumentException("UserId là bắt buộc.", nameof(review.UserId));

            if (review.Rating < 1 || review.Rating > 5)
                throw new ArgumentOutOfRangeException(nameof(review.Rating),
                    "Đánh giá phải nằm trong khoảng từ 1 đến 5 sao.");

            if (string.IsNullOrWhiteSpace(review.Comment))
                throw new ArgumentException("Nội dung đánh giá không được để trống.", nameof(review.Comment));


            // Validate business rule
            var hasPurchased = await _orderRepo.HasUserPurchasedProductAsync(review.UserId, review.ProductId);
            if (!hasPurchased)
                throw new Exception("Chỉ khách đã mua hàng thành công mới được đánh giá.");

            var hasReviewed = await _reviewRepo.HasUserReviewedProductAsync(review.ProductId, review.UserId);
            if (hasReviewed)
                throw new Exception("Bạn đã đánh giá sản phẩm này rồi.");

            review.CreatedAt = DateTime.UtcNow;

            await _reviewRepo.AddAsync(review);

            return review;
        }

    }
}