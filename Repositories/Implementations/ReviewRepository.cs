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

        public List<Review> GetByProductId(int productId)
        {
            return _context.Reviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }
        public async Task AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

    }
}
