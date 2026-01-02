using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.Models;
using MyWeb.Repositories.Interfaces;

namespace MyWeb.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly TechZoneDbContext _context;

        public ProductRepository(TechZoneDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllActive()
        {
            return _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Attributes)
                .ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Attributes)
                .FirstOrDefault(p => p.Id == id && p.IsActive);

        }

        public IEnumerable<Product> GetFeatured(int take)
        {
            return _context.Products
                .Where(p => p.IsActive && p.IsFeatured)
                .Include(p => p.Category)
                .Include(p => p.Attributes)
                .OrderByDescending(p => p.Id)
                .Take(take)
                .ToList();
        }

        public IEnumerable<Product> GetRelatedProducts(int categoryId, int excludeProductId, int take)
        {
            return _context.Products
                .Where(p =>
                    p.IsActive &&
                    p.CategoryId == categoryId &&
                    p.Id != excludeProductId)
                .Include(p => p.Attributes)
                .OrderByDescending(p => p.IsFeatured)
                .Take(take)
                .ToList();
        }

        public IEnumerable<Product> GetByFilter(string type, string value)
        {
            var query = _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Attributes)
                .AsQueryable();

            switch (type.ToLower())
            {
                case "category":                 
                    query = query.Where(p => p.Category.Name == value);
                    break;
                case "brand":
                    query = query.Where(p => p.Brand.Name == value);
                    break;
                case "attribute":
                    query = query.Where(p => p.Attributes.Any(a => a.Value == value));
                    break;
            }

            return query.ToList();  
        }

        //public async Task<bool> ReduceStockAndIncreaseSoldAsync(int productId, int quantity)
        //{
        //    // check stock khi update
        //    string sql = "UPDATE Product SET Stock = Stock - {0}, Sold = Sold + {0} WHERE Id = {1} AND Stock >= {0}";

        //    int result = await _context.Database.ExecuteSqlRawAsync(sql, quantity, productId);

        //    return result > 0;
        //}  chuyển qua order repo
    }
}
