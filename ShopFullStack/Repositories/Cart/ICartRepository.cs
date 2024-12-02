using ShopFullStack.Models;

public interface ICartRepository
{
    Task<Cart?> GetByIdAsync(long id, long customerId); 
    Task<Cart> AddAsync(Cart cart); 
    Task ClearCartAsync(Cart cart);
    Task<Cart?> GetByCustomerIdAsync(long customerId); 
    Task<Cart?> AddItemToCartAsync(long cartId, CartItem cartItem, long customerId);
    Task<Cart?> RemoveItemFromCartAsync(long cartId, long cartItemId, long customerId); 
}