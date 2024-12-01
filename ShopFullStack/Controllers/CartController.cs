using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopFullStack.Models;
using ShopFullStack.Services;

namespace ShopFullStack.Controllers;

public class CartController: Controller
{
    
    private readonly CartService _cartService;
    private readonly CustomerService _customerService;
    private readonly UserManager<IdentityUser> _userManager;
    
    public CartController(
        CartService cartService, 
        UserManager<IdentityUser> userManager,
        CustomerService customerService
    )
    {
        _userManager = userManager;
        _cartService = cartService;
        _customerService = customerService;
    }
    
    [HttpPost]
    public IActionResult Checkout()
    {
        return RedirectToAction("OrdersPage", "Order");
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(long productId, int quantity)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity", ReturnUrl = Url.Action("CartPage", "Cart") });
        }
        
        var customer = await GetCurrentCustomer();
        if (customer == null)
        {
            return RedirectToAction("UpdateCustomer", "Customer");
        }
        var cartByCustomerId = await _cartService
            .GetCartByCustomerId(customer.Id);
        
        CartItem cartItem = new CartItem();
        cartItem.ProductId = productId;
        cartItem.Quantity = quantity;
        
        if (cartByCustomerId.Data == null)
        {
            Cart newCart = new Cart();
            newCart.CustomerId = customer.Id;
            var newCartResponse = await  _cartService.CreateCart(newCart);

            var savedCart = newCartResponse.Data;
            if (savedCart != null)
            {
               await _cartService.AddItemToCart(savedCart, cartItem);
            }
        }
        else
        {
            await _cartService.AddItemToCart(cartByCustomerId.Data, cartItem);
        }
        return RedirectToAction("CartPage", "Cart");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveItem(long cartItemId)
    {
        var customer = await GetCurrentCustomer();
        if (customer == null)
        {
            return RedirectToAction("CartPage", "Cart");
        }
        var cartByCustomerId = await _cartService
            .GetCartByCustomerId(customer.Id);
        
        if (cartByCustomerId.Data != null)
        {
           await _cartService
               .RemoveItemFromCart(cartByCustomerId.Data.Id, cartItemId);
        }
        return RedirectToAction("CartPage", "Cart");
    }
    
    
    [HttpGet]
    public async Task<IActionResult>  CartPage()
    {  
        var customer = await GetCurrentCustomer();
        if (customer == null)
        {
            return View(new Cart());
        }
        var response = await _cartService.GetCartByCustomerId(customer.Id);
        return View(response.Data);
    }

    private async Task<Customer?> GetCurrentCustomer()
    {
        var user = _userManager.GetUserAsync(User).Result;
        var email = user == null? HttpContext.Session.GetString("Email") : user.Email;
        var customerResponse = await _customerService.GetCustomerByEmail(email);
        return customerResponse.Data;
    }
    
}