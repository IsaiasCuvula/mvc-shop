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
    
    public async Task<ApiResponse<Cart>> RemoveItemFromCart(long cartId, long cartItemId)
    {
        ApiResponse<Cart> response = new ApiResponse<Cart>();
        try
        {
            var result=   await _cartRepository
                .RemoveItemFromCartAsync(cartId,cartItemId);
            
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
    
    public async Task<ApiResponse<Cart>> AddItemToCart(Cart cart, CartItem cartItem)
    {
        ApiResponse<Cart> response = new ApiResponse<Cart>();
        Cart? newCart = new Cart();
        try
        {
            cartItem.CartId = cart.Id;
            cartItem.Total = await GetTotalByProduct(cartItem);
            //
            var productInCart = cart.CartItems
                .FirstOrDefault(x => x.ProductId == cartItem.ProductId);
            
            if (productInCart == null)
            {
                newCart = await _cartRepository
                    .AddItemToCartAsync(cartItem.CartId, cartItem);
            }
            else
            {
                productInCart.Quantity += cartItem.Quantity;
                productInCart.Total += cartItem.Total;
                
                newCart =  await _cartRepository.
                    AddItemToCartAsync(cartItem.CartId, productInCart);
            }
           
            response.Data = newCart;
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

    public async Task<ApiResponse<Cart>>  ClearCartById(long id)
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
            cart.CartItems.Clear();
            await _cartRepository.ClearCartAsync(cart);
           
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