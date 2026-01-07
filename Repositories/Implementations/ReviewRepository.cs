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
            var skip = (page - 1) * pageSize;
            return await _context.Reviews
                .AsNoTracking()
                .Include(r => r.User) // Join bảng User để lấy FullName
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt) // Mới nhất lên đầu
                .Skip(skip) // Bỏ qua các trang trước
                .Take(pageSize) // Lấy số lượng cần thiết
                .ToListAsync();
        }


        public async Task<List<Review>> GetAllReviewsByProductIdAsync(int productId)
        {
            return await _context.Reviews
                .AsNoTracking()
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }

        public async Task AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasUserReviewedProductAsync(int productId, string userId)
        {
            return await _context.Reviews.AsNoTracking()
                .AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }

    }
}
