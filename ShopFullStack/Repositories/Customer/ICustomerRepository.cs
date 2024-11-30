using ShopFullStack.Models;

namespace ShopFullStack.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetCustomerByEmail(string email);
    Task<Customer?> GetByIdAsync(long id);
    Task<List<Customer>> GetAllAsync();
    Task<Customer> AddAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
    Task DeleteAsync(Customer customer);
    Task<List<Customer>> GetAllCustomerShoppedLasWeekAsync();
    Task<Customer?> GetByNumberAsync(long customerNumber);
}