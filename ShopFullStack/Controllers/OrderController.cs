using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopFullStack.Models;
using ShopFullStack.Services;
using ShopFullStack.Utilities;

namespace ShopFullStack.Controllers;

public class OrderController: Controller
{
    
    private readonly OrderService _orderService;
    private readonly CartService _cartService;
    private readonly CustomerService _customerService;
    private readonly UserManager<IdentityUser> _userManager;
    
    public OrderController(
        OrderService orderService, 
        UserManager<IdentityUser> userManager,
        CustomerService customerService,
        CartService cartService
    )
    {
        _userManager = userManager;
        _orderService = orderService;
        _customerService = customerService;
        _cartService = cartService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(long id)
    {
        Console.WriteLine($"orderId: {id}");
        var customer = await GetCurrentCustomer();
        if (customer == null)
        {
            return RedirectToAction("OrdersPage", "Order");
        }
        var response = await _orderService.GetOrderById(id, customer.Id);
        var order = response.Data;
        if (order != null)
        {
            Console.WriteLine($"OrderItems: {order.OrderItems.Count}");
        }
        return View(order ?? new Order());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(long id, OrderStatus status)
    {
        // var response = await _orderService.GetOrderById(orderId);
        // if (order == null)
        // {
        //     return NotFound();
        // }
        //
        // order.Status = status;
        // _context.Update(order);
        // await _context.SaveChangesAsync();

        //return RedirectToAction(nameof(Details), new { id = order.Id });
        return RedirectToAction(nameof(Details), new { id = 2 });
    }
    
    [HttpPost]
    public async Task<IActionResult> Checkout(long cartId)
    {
        var customer = await GetCurrentCustomer();

        if (customer == null)
        {
            return RedirectToAction("CartPage", "Cart");
        }
        
        var customerId = customer.Id;
        
        var response = await _cartService.GetCartById(cartId, customerId);
        var cart = response.Data;
        if (cart == null)
        {
            return RedirectToAction("CartPage", "Cart");
        }
        else
        {
            var customerResponse = await _customerService
                .GetCustomerById(cart.CustomerId);
            
            var order = new Order();
            order.CustomerId = cart.CustomerId;
            order.CartId = cartId;
            order.OrderItems = cart.CartItems;
            order.Total = cart.CartItems.Sum(ci => ci.Total);
            if (customerResponse.Data != null)
            {
                order.ShippingAddress = customerResponse.Data.Address;
                order.Customer = customerResponse.Data;
            }
            await _orderService.CreateOrder(order);
            //After placing order clear the cart
            await _cartService.ClearCartById(cartId, customerId);
            
            return RedirectToAction("OrdersPage", "Order");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult>  OrdersPage()
    {  
        var user = _userManager.GetUserAsync(User).Result;
        var email = user == null?  HttpContext.Session.GetString("Email"): user.Email;
        var customerResponse = await  _customerService.GetCustomerByEmail(email);
        var customer = customerResponse.Data;
        if (customer != null){
            var response = await _orderService.GetAllOrders(customer.Id);
            return View(response.Data ?? new List<Order>());
        }
        else
        {
            return View(new List<Order>());
        }
    }
 
    private async Task<Customer?> GetCurrentCustomer()
    {
        var user = _userManager.GetUserAsync(User).Result;
        var email = user == null? HttpContext.Session.GetString("Email") : user.Email;
        var customerResponse = await _customerService.GetCustomerByEmail(email);
        return customerResponse.Data;
    }
    
}