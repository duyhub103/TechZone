using MyWeb.Models;

namespace MyWeb.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        // Lấy danh sách review có phân trang
        Task<List<Review>> GetReviewsByProductAsync(int productId, int page, int pageSize);

        // Lấy toàn bộ review (để tính toán thống kê sao)
        Task<List<Review>> GetAllReviewsByProductIdAsync(int productId);

        Task AddAsync(Review review);

        //check user đã review sp chưa
        Task<bool> HasUserReviewedProductAsync(int productId, string userId);
    }
}
