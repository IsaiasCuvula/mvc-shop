using ShopFullStack.Models;

namespace ShopFullStack.Repositories.Orders;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(long id, long customerId);
    Task<Order?> AdminGetByIdAsync(long id);
    Task<List<Order>> GetAllAsync(long customerId);
    Task<List<Order>> AdminGetAllAsync();
    Task<Order> AddAsync(Order order);
    Task<Order> UpdateAsync(Order order);
    Task DeleteAsync(Order order);
    Task<List<Order>> GetMostPopularAsync();
    Task<List<Order>> GetAllUnpaidOrdersAsync();
    Task<List<Order>> GetAllReturnedOrdersAsync();
}