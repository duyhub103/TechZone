using MyWeb.ViewModels;
using MyWeb.Models;

namespace MyWeb.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts(string? type = null, string? value = null);
        Task<ProductDetailViewModel> GetProductDetailAsync(int id);

        // Hàm phục vụ AJAX Load More
        Task<List<Review>> GetMoreReviewsAsync(int productId, int page, int pageSize);
    }
}
