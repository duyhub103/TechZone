using MyWeb.Models;
using MyWeb.ViewModels; // Nếu cần

namespace MyWeb.Repositories.Interfaces
{
    public interface IProductRepository
    {

        Task<PaginatedList<Product>> GetProductsAsync(string? keyword, string? type, string? value, int pageIndex, int pageSize);

        Task<IEnumerable<Product>> SearchLiveAsync(string keyword);

        Task<Product?> GetByIdAsync(int id);
        Task<List<Product>> GetFeaturedAsync(int take);
        Task<List<Product>> GetRelatedProductsAsync(int categoryId, int excludeProductId, int take);

    }
}