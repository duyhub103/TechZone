using MyWeb.Models;
using MyWeb.ViewModels;

namespace MyWeb.Services.Interfaces
{
    public interface IProductService
    {
        Task<PaginatedList<Product>> GetAllProductsAsync(string? keyword, string? type, string? value, int pageIndex = 1);

        Task<IEnumerable<Product>> SearchLiveAsync(string keyword);

        Task<ProductDetailViewModel> GetProductDetailAsync(int id);

        //ajax load more
        Task<List<Review>> GetMoreReviewsAsync(int productId, int page, int pageSize);
    }
}