using MyWeb.Models;
using MyWeb.ViewModels;

namespace MyWeb.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllActive();
        Product? GetById(int id);
        IEnumerable<Product> GetFeatured(int take);
        IEnumerable<Product> GetByFilter(string type, string value);
        IEnumerable<Product> GetRelatedProducts(int categoryId, int excludeProductId, int take);

        PaginatedList<Product> GetProducts(string? search, string? type, string? value, int pageIndex, int pageSize);
        Task<LiveSearchViewModel> SearchAsync(string query);
    }
}
