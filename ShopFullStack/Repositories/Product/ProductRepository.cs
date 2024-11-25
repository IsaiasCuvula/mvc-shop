using Microsoft.EntityFrameworkCore;
using ShopApi.Data;

namespace ShopApi.Repositories.Product;
using Models; 

public class ProductRepository: IProductRepository
{
    private readonly AppDbContext _context;

    public  ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Product>> GetExpiredProductsAsync()
    {
        var today = DateTime.UtcNow.ToUniversalTime();
        
        var query = @"
                    SELECT * FROM Products p
                    WHERE p.expiration_date <= {0}";
        
        Console.WriteLine("**********************************");
        Console.WriteLine($"Today: {today}");
        Console.WriteLine("**********************************");

        return await _context.Products
            .FromSqlRaw(query, today)
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsExpiringInNext24HoursAsync()
    {
        var now = DateTime.UtcNow.ToUniversalTime();
        var dateAfter24Hours = now.AddHours(24).ToUniversalTime();
        
        var query = @"
            SELECT * FROM Products p
            WHERE p.expiration_date BETWEEN {0} AND {1}";
        
        Console.WriteLine("**********************************");
        Console.WriteLine($"now: {now}");
        Console.WriteLine($"Date after 24 hours: {dateAfter24Hours}");
        Console.WriteLine("**********************************");

        return await _context.Products
            .FromSqlRaw(query, now, dateAfter24Hours)
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsExpiringInNext3MonthsAsync()
    {
        var now = DateTime.UtcNow.ToUniversalTime();
        var dateAfter3Months = now.AddMonths(3).ToUniversalTime();
        
        var query = @"
                    SELECT * FROM Products p
                    WHERE p.expiration_date > {0}";
        
        Console.WriteLine("**********************************");
        Console.WriteLine($"now: {now}");
        Console.WriteLine($"Date after 3 months: {dateAfter3Months}");
        Console.WriteLine("**********************************");

        return await _context.Products
            .FromSqlRaw(query, dateAfter3Months)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(long id)
    {
        return await _context.Products.FirstOrDefaultAsync(c=> c.Id==id);
    }

    public async Task<Product?> GetByNumberAsync(long productNumber)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p=> p.ProductNumber == productNumber);
    }

    public  async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }
    
    public  async Task<Product> AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async  Task<Product> UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async  Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}