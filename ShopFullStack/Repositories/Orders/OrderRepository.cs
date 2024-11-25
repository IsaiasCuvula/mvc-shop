using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Models;
using ShopApi.Utilities;

namespace ShopApi.Repositories.Orders;

public class OrderRepository: IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Order?> GetByIdAsync(long id)
    {
        return await _context.Orders.FirstOrDefaultAsync(c=> c.Id==id);
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order> AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task DeleteAsync(Order order)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Order>> GetAllUnpaidOrdersAsync()
    {
        return await _context.Orders
            .FromSql($"SELECT * FROM Orders WHERE payment_status = {PaymentStatus.Unpaid}")
            .ToListAsync();
    }

    public async Task<List<Order>> GetAllReturnedOrdersAsync()
    {
        return await _context.Orders
            .FromSql($"SELECT * FROM Orders WHERE return_status = {ReturnStatus.Returned}")
            .ToListAsync();
    }
}