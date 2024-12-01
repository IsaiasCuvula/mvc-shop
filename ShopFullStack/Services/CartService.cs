using ShopFullStack.Models;
using ShopFullStack.Repositories.Product;

namespace ShopFullStack.Services;

public class CartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }
    
    public async Task<ApiResponse<Cart>> GetCartByCustomerId(long customerId)
    {
        ApiResponse<Cart> response = new ApiResponse<Cart>();
        try
        {
            var result=   await _cartRepository
                .GetByCustomerIdAsync(customerId);
            
            response.Data = result;
            response.Message = "Cart fetched successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to fetch cart by customer id: {customerId} - {e}");
            return response;
        }
    }

    
    
    public async Task<ApiResponse<Cart>> RemoveItemFromCart(long cartId, CartItem cartItem)
    {
        ApiResponse<Cart> response = new ApiResponse<Cart>();
        try
        {
            var result=   await _cartRepository
                .RemoveItemFromCartAsync(cartId, cartItem.CartItemId);
            
            response.Data = result;
            response.Message = "Item removed successfully from cart";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to remove item from cart - {e}");
            return response;
        }
    }
    
    public async Task<ApiResponse<Cart>> AddItemToCart(long cartId, CartItem cartItem)
    {
        ApiResponse<Cart> response = new ApiResponse<Cart>();
        try
        {
            cartItem.Total = await GetTotalByProduct(cartItem);
            //
            var result=   await _cartRepository.AddItemToCartAsync(cartId, cartItem);
            response.Data = result;
            response.Message = "Item added successfully to cart";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to add item to cart - {e}");
            return response;
        }
    }
    
    
    
    public async Task<ApiResponse<Cart>>  CreateCart(Cart cart)
    {
        ApiResponse<Cart> response = new ApiResponse<Cart>();
        try
        {
            var result = await _cartRepository.AddAsync(cart);
            response.Data = result;
            response.Message = "Cart created successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to create cart - {e}");
            return response;
        }
    }
   
    public async Task<ApiResponse<Cart>> GetCartById(long id)
    {
        ApiResponse<Cart> response = new ApiResponse<Cart>();
        try
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null)
            {
                response.Message = "Cart not found";
                return response;
            }
            response.Data = cart;
            response.Message = "Cart successfully retrieved";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get cart with id: {id} - {e}");
            return response;
        }
    }

    public async Task<ApiResponse<Cart>>  DeleteCartById(long id)
    {
        ApiResponse<Cart> response = new ApiResponse<Cart>();
        try
        {
            var cartItem = await _cartRepository.GetByIdAsync(id);
            
            if (cartItem == null)
            {
                response.Message = "Cart not found";
                return response;
            }
            await _cartRepository.DeleteAsync(cartItem);
           
            response.Data = null;
            response.Message = "Cart deleted successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get cartItem with id: {id} - {e}");
            return response;
        }
    }
    
    
    private async Task<decimal> GetTotalByProduct(CartItem cartItem)
    {
        var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
        if (product == null){return 0;}
        return product.Price * cartItem.Quantity;
    }

}