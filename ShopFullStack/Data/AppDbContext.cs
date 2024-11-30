using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopFullStack.Models;

namespace ShopFullStack.Data;

public class AppDbContext: IdentityDbContext<IdentityUser>
{

    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    
}