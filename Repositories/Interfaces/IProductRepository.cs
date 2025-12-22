using MyWeb.Models;

namespace MyWeb.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllActive();
        Product? GetById(int id);
        IEnumerable<Product> GetFeatured(int take);
        IEnumerable<Product> GetByFilter(string type, string value);
        IEnumerable<Product> GetRelatedProducts(int categoryId, int excludeProductId, int take);
    }
}
