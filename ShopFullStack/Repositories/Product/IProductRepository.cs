
namespace ShopFullStack.Repositories.Product;
using Models; 

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(long id);
    Task<Product> AddAsync(Product customer);
    Task<Product> UpdateAsync(Product customer);
    Task DeleteAsync(Product customer);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> GetExpiredProductsAsync();
    Task<List<Product>> GetProductsExpiringInNext24HoursAsync();
    Task<List<Product>> GetProductsExpiringInNext3MonthsAsync();
}