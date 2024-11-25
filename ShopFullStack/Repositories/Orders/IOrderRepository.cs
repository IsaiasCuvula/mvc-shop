using ShopApi.Models;

namespace ShopApi.Repositories.Orders;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(long id);
    Task<List<Order>> GetAllAsync();
    Task<Order> AddAsync(Order order);
    Task<Order> UpdateAsync(Order order);
    Task DeleteAsync(Order order);
    Task<List<Order>> GetAllUnpaidOrdersAsync();
    Task<List<Order>> GetAllReturnedOrdersAsync();
}