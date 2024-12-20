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
        var response = new ApiResponse<Cart>();
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
    
    public async Task RemoveItemFromCart(
        long cartId, long cartItemId,long customerId)
    {
        try
        {
             await _cartRepository
                .RemoveItemFromCartAsync(cartId,cartItemId,customerId);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to remove item from cart - {e}");
        }
    }
    
    public async Task AddItemToCart(
        Cart cart, CartItem cartItem,long customerId)
    {
        try
        {
            cartItem.CartId = cart.Id;
            cartItem.Total = await GetTotalByProduct(cartItem);
            //
            var productInCart = cart.CartItems
                .FirstOrDefault(x => x.ProductId == cartItem.ProductId);
            
            if (productInCart == null)
            {
                await _cartRepository
                    .AddItemToCartAsync(cartItem.CartId, cartItem, customerId);
            }
            else
            {
                productInCart.Quantity += cartItem.Quantity;
                productInCart.Total += cartItem.Total;
                await _cartRepository.
                    AddItemToCartAsync(cartItem.CartId, productInCart, customerId);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to add item to cart - {e}");
        }
    }
    
    public async Task<ApiResponse<Cart>>  CreateCart(Cart cart)
    {
        var response = new ApiResponse<Cart>();
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
   
    public async Task<ApiResponse<Cart>> GetCartById(long id, long customerId)
    {
        var response = new ApiResponse<Cart>();
        try
        {
            var cart = await _cartRepository.GetByIdAsync(id, customerId);
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

    public async Task  ClearCartById(long id, long customerId)
    {
        try
        {
            var cart = await _cartRepository.GetByIdAsync(id, customerId);
            
            if (cart != null)
            {
                await UpdateProductStockQty(cart);
                cart.CartItems.Clear();
                await _cartRepository.ClearCartAsync(cart);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get cartItem with id: {id} - {e}");
        }
    }
    
    private async Task UpdateProductStockQty(Cart cart)
    {
        foreach (var item in cart.CartItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                product.Stock -= item.Quantity;
                await _productRepository.UpdateAsync(product);
            }
        }
    }
    
    private async Task<decimal> GetTotalByProduct(CartItem cartItem)
    {
        var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
        if (product == null){return 0;}
        return product.Price * cartItem.Quantity;
    }

}