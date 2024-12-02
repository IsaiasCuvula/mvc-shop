using Microsoft.EntityFrameworkCore;
using ShopFullStack.Data;
using ShopFullStack.Models;
using ShopFullStack.Utilities;

namespace ShopFullStack.Repositories;

public class CustomerRepository: ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllCustomerShoppedLasWeekAsync()
    {
        var lastWeekMonday = AppHelpers.GetLastWeekMonday();
        var lastWeekSunday = lastWeekMonday.AddDays(6);
        
        var query = @"
        SELECT DISTINCT c.*
        FROM Orders o
        JOIN Customers c ON o.customer_id = c.Id
        WHERE o.created_at BETWEEN {0} AND {1}";

        return await _context.Customers
            .FromSqlRaw(query, lastWeekMonday, lastWeekSunday)
            .ToListAsync();
    }
    
    public async Task<Customer?> GetByNumberAsync(long customerNumber)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c=> c.CustomerNumber == customerNumber);
    }

    public async Task<Customer?> GetCustomerByEmail(string email)
    {
        return await _context.Customers.FirstOrDefaultAsync(c=> c.Email == email);
    }

    public async Task<Customer?> GetByIdAsync(long id)
    {
     return await _context.Customers.FirstOrDefaultAsync(c=> c.Id==id);
    }

    public  async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public  async Task<Customer> AddAsync(Customer customer)
    {
       await _context.Customers.AddAsync(customer);
       await _context.SaveChangesAsync();
       return customer;
    }

    public async  Task<Customer> UpdateAsync(Customer customer)
    {
         _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async  Task DeleteAsync(Customer customer)
    {
         _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}