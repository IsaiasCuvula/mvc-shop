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
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Cart> Carts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 
        
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.Customer)
            .WithOne(cu => cu.Cart)
            .HasForeignKey<Cart>(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer) 
            .WithMany(cu => cu.Orders) 
            .HasForeignKey(o => o.CustomerId) 
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(ci => ci.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

    }
    
}