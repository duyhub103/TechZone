using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.Models;
using MyWeb.Repositories.Interfaces;

namespace MyWeb.Repositories.Implementations
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly TechZoneDbContext _context;

        public ReviewRepository(TechZoneDbContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetReviewsByProductAsync(int productId, int page, int pageSize)
        {
            return await _context.Reviews
                .Include(r => r.User) // Join bảng User để lấy FullName
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt) // Mới nhất lên đầu
                .Skip((page - 1) * pageSize) // Bỏ qua các trang trước
                .Take(pageSize) // Lấy số lượng cần thiết
                .ToListAsync();
        }


        public async Task<List<Review>> GetAllReviewsByProductIdAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }

        public async Task AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

    }
}
