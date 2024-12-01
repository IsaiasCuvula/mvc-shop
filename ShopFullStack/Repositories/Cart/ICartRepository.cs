using ShopFullStack.Models;

public interface ICartRepository
{
    Task<Cart?> GetByIdAsync(long id); 
    Task<Cart> AddAsync(Cart cart); 
    Task DeleteAsync(Cart cart);
    Task<Cart?> GetByCustomerIdAsync(long customerId); 
    Task<Cart?> AddItemToCartAsync(long cartId, CartItem cartItem); 
    Task<Cart?> RemoveItemFromCartAsync(long cartId, long cartItemId); 
}