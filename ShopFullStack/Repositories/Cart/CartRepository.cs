using Microsoft.EntityFrameworkCore;
using ShopFullStack.Data;
using ShopFullStack.Models;

namespace ShopFullStack.Repositories;

public class CartRepository: ICartRepository
{
    
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Cart?> UpdateItemInCartAsync(long cartId, CartItem cartItem)
    {
        var cart = await GetByIdAsync(cartId);
        if (cart != null)
        {
            cart.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
        }
        return cart;
    }

    public async Task<Cart?> AddItemToCartAsync(long cartId, CartItem cartItem)
    {
        var cart = await GetByIdAsync(cartId);
        if (cart != null)
        {
            cart.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
        }
        return cart;
    }

    public async Task<Cart?> RemoveItemFromCartAsync(long cartId, long cartItemId)
    {
        var cart = await GetByIdAsync(cartId);
        if (cart != null)
        {
            var item = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);
            if (item != null)
            {
                cart.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
        return cart;
    }


    public async Task<Cart?> GetByIdAsync(long cartId)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId);
    }

    public async Task<Cart?> GetByCustomerIdAsync(long customerId)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

    public async Task<Cart> AddAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task DeleteAsync(Cart cart)
    {
        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
    }
}