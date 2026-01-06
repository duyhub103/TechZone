using MyWeb.Models;
using MyWeb.ViewModels; // Nếu cần

namespace MyWeb.Repositories.Interfaces
{
    public interface IProductRepository
    {

        PaginatedList<Product> GetProducts(string? keyword, string? type, string? value, int pageIndex, int pageSize);

        Task<IEnumerable<Product>> SearchLiveAsync(string keyword);

        Product? GetById(int id);
        IEnumerable<Product> GetFeatured(int take);
        IEnumerable<Product> GetRelatedProducts(int categoryId, int excludeProductId, int take);

    }
}