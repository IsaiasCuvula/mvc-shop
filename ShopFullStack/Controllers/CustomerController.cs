using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopFullStack.Models;
using ShopFullStack.Services;

namespace ShopFullStack.Controllers;

public class CustomerController: Controller
{
    
    private readonly CustomerService _customerService;
    private readonly UserManager<IdentityUser> _userManager;

    public CustomerController(CustomerService customerService, 
        UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
        _customerService = customerService;
    }
    
    [HttpGet]
    public IActionResult CompleteProfile()
    {
        var user = _userManager.GetUserAsync(User).Result;

        if (user != null)
        {
            ViewData["Email"] = user.Email;
            ViewData["AppUserId"] = user.Id;
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CompleteProfile(Customer customer)
    {
        var user = await _userManager.GetUserAsync(User);
        Console.WriteLine("********************************************");
        Console.WriteLine($"user: {user}");
        Console.WriteLine("********************************************");
        if (user != null)
        {
            customer.Email = User.Identity.Name;
            //customer.AppUserId = User.Identity;
        }
     
        if (!ModelState.IsValid)
        {
            Console.WriteLine(string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            Console.WriteLine("********************************************");
            Console.WriteLine($"Saving customer: {customer.Name}, {customer.AppUserId} {customer.Email}, {customer.Phone}");
            Console.WriteLine("********************************************");
            // var user = await _context.Users.FindAsync(User.Identity.Name); // Pega o usuário logado
            // var customer = model.Customer;
            //
            // // Atualiza as informações do cliente
            // customer.AppUserId = user.Id;
            // _context.Customers.Add(customer);
            // await _context.SaveChangesAsync();

            //return RedirectToAction("Index", "Home");
            return View(customer);
        }
        return View(customer);
    }
    
    // [HttpGet("top-customer")]
    // public async Task<ActionResult<ApiResponse<Customer>>> GetTopCustomerByTurnover()
    // {
    //     var customers = await _customerService.GetTopCustomerByTurnover();
    //     return Ok(customers);
    // }
    //
    // [HttpGet("shopped-last-week")]
    // public async Task<ActionResult<ApiResponse<List<Customer>>>> GetAllCustomerShoppedLasWeek()
    // {
    //     var customers = await _customerService.GetAllCustomerShoppedLasWeek();
    //     return Ok(customers);
    // }
    //
    // [HttpPut("{id}")]
    // public async Task<ActionResult<ApiResponse<Customer>>> UpdateCustomer(long id, CustomerDto dto)
    // {
    //     return Ok(await _customerService.UpdateCustomer(dto, id));
    // }
    //
    // [HttpPost]
    // public async Task<ActionResult<ApiResponse<Customer>>> CreateCustomer(CustomerDto dto)
    // {
    //     return Ok(await _customerService.CreateCustomer(dto));
    // }
    //
    // [HttpGet("{id}")]
    // public async Task<ActionResult<ApiResponse<Customer>>> GetCustomerById(long id)
    // {
    //     return Ok(await _customerService.GetCustomerById(id));
    // }
    //
    // [HttpGet("all")]
    // public async Task<ActionResult<ApiResponse<List<Customer>>>> GetAllCustomers()
    // {
    //     var customers = await _customerService.GetAllCustomers();
    //     return Ok(customers);
    // }
    //
    // [HttpDelete("{id}")]
    // public async Task<ActionResult<ApiResponse<Customer>>> DeleteCustomerById(long id)
    // {
    //     return Ok(await _customerService.DeleteCustomerById(id));
    // }
    
}