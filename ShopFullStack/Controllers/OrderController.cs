using Microsoft.AspNetCore.Mvc;
using ShopApi.Dtos;
using ShopApi.Models;
using ShopApi.Services;

namespace ShopApi.Controllers;

[Route("api/v1/orders")]
[ApiController]
public class OrderController: ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpGet("unpaid")]
    public async Task<ActionResult<ApiResponse<List<Order>>>> GetAllUnpaidOrders()
    {
        var orders = await _orderService.GetAllUnpaidOrders();
        return Ok(orders);
    }
    
    [HttpGet("returned")]
    public async Task<ActionResult<ApiResponse<List<Order>>>> GetAllReturnedOrders()
    {
        var orders = await _orderService.GetAllReturnedOrders();
        return Ok(orders);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Order>>> UpdateOrder(long id, OrderDto dto)
    {
        return Ok(await _orderService.UpdateOrder(dto, id));
    }
    
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Order>>> CreateOrder(UserOrdersDto dto)
    {
        return Ok(await _orderService.CreateOrder(dto));
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Order>>> GetOrderById(long id)
    {
        return Ok(await _orderService.GetOrderById(id));
    }

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<List<Order>>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrders();
        return Ok(orders);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Order>>> DeleteOrderById(long id)
    {
        return Ok(await _orderService.DeleteOrderById(id));
    }
}