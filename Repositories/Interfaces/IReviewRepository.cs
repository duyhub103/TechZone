using MyWeb.Models;

namespace MyWeb.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        List<Review> GetByProductId(int productId);
        Task AddAsync(Review review);
    }
}
