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

    [HttpGet]
    public IActionResult UpdateCart()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult RemoveItem()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Checkout()
    {
        return View();
    }

    
    [HttpGet]
    public async Task<IActionResult>  CartPage()
    {  
        var user = _userManager.GetUserAsync(User).Result;
        var email = HttpContext.Session.GetString("Email");
        var appUserId = HttpContext.Session.GetString("AppUserId");
        var customerResponse = await _customerService.GetCustomerByEmail(email);
        var customer = customerResponse.Data;
        if (customer == null)
        {
            return View(new Cart());
        }
        var response = await _cartService.GetByCustomerId(customer.Id);
        return View(response.Data);
    }
    
}