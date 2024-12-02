using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopFullStack.Models;
using ShopFullStack.Services;

namespace ShopFullStack.Controllers;

public class OrderController: Controller
{
    
    private readonly OrderService _orderService;
    private readonly CustomerService _customerService;
    private readonly UserManager<IdentityUser> _userManager;
    
    public OrderController(
        OrderService orderService, 
        UserManager<IdentityUser> userManager,
        CustomerService customerService
    )
    {
        _userManager = userManager;
        _orderService = orderService;
        _customerService = customerService;
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
    //
    // [HttpGet("unpaid")]
    // public async Task<ActionResult<ApiResponse<List<Order>>>> GetAllUnpaidOrders()
    // {
    //     var orders = await _orderService.GetAllUnpaidOrders();
    //     return Ok(orders);
    // }
    //
    // [HttpGet("returned")]
    // public async Task<ActionResult<ApiResponse<List<Order>>>> GetAllReturnedOrders()
    // {
    //     var orders = await _orderService.GetAllReturnedOrders();
    //     return Ok(orders);
    // }
    //
    // [HttpPut("{id}")]
    // public async Task<ActionResult<ApiResponse<Order>>> UpdateOrder(long id, OrderDto dto)
    // {
    //     return Ok(await _orderService.UpdateOrder(dto, id));
    // }
    //
    // [HttpPost]
    // public async Task<ActionResult<ApiResponse<Order>>> CreateOrder(UserOrdersDto dto)
    // {
    //     return Ok(await _orderService.CreateOrder(dto));
    // }
    //
    // [HttpGet("{id}")]
    // public async Task<ActionResult<ApiResponse<Order>>> GetOrderById(long id)
    // {
    //     return Ok(await _orderService.GetOrderById(id));
    // }
    //
    // [HttpGet("all")]
    // public async Task<ActionResult<ApiResponse<List<Order>>>> GetAllOrders()
    // {
    //     var orders = await _orderService.GetAllOrders();
    //     return Ok(orders);
    // }
    //
    // [HttpDelete("{id}")]
    // public async Task<ActionResult<ApiResponse<Order>>> DeleteOrderById(long id)
    // {
    //     return Ok(await _orderService.DeleteOrderById(id));
    // }
}