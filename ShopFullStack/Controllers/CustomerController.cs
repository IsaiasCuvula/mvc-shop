using Microsoft.AspNetCore.Mvc;
using ShopApi.Dtos;
using ShopApi.Models;
using ShopApi.Services;

namespace ShopApi.Controllers;

[Route("api/v1/customers")]
[ApiController]
public class CustomerController: ControllerBase
{
    
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }
    
    [HttpGet("top-customer")]
    public async Task<ActionResult<ApiResponse<Customer>>> GetTopCustomerByTurnover()
    {
        var customers = await _customerService.GetTopCustomerByTurnover();
        return Ok(customers);
    }
    
    [HttpGet("shopped-last-week")]
    public async Task<ActionResult<ApiResponse<List<Customer>>>> GetAllCustomerShoppedLasWeek()
    {
        var customers = await _customerService.GetAllCustomerShoppedLasWeek();
        return Ok(customers);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Customer>>> UpdateCustomer(long id, CustomerDto dto)
    {
        return Ok(await _customerService.UpdateCustomer(dto, id));
    }
    
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Customer>>> CreateCustomer(CustomerDto dto)
    {
        return Ok(await _customerService.CreateCustomer(dto));
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Customer>>> GetCustomerById(long id)
    {
        return Ok(await _customerService.GetCustomerById(id));
    }

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<List<Customer>>>> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomers();
        return Ok(customers);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Customer>>> DeleteCustomerById(long id)
    {
        return Ok(await _customerService.DeleteCustomerById(id));
    }
    
}