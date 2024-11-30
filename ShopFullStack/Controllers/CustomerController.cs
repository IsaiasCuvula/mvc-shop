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
    public async Task<IActionResult> UpdateCustomer()
    {  
        var user = _userManager.GetUserAsync(User).Result;
        var email = HttpContext.Session.GetString("Email");
        var appUserId = HttpContext.Session.GetString("AppUserId");
        ViewData["Email"] = user == null? email : user.Email;
        ViewData["AppUserId"] = user == null? appUserId: user.Id;
        var result = await _customerService
            .GetCustomerByAppUserId(appUserId);
        return View(result.Data);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCustomer(Customer customer)
    {
        Console.WriteLine($"Saving customer: {customer.Name}, {customer.AppUserId} {customer.Email}, {customer.Phone}");
        
        if (!ModelState.IsValid)
        {
            Console.WriteLine(string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return View(customer);
        }
        else
        {
            await _customerService.UpdateCustomer(customer);
            return View(customer);
        }
    }
    
    [HttpGet]
    public IActionResult CompleteProfile()
    {  
        var user = _userManager.GetUserAsync(User).Result;
       var email = HttpContext.Session.GetString("Email");
       var appUserId = HttpContext.Session.GetString("AppUserId");

       ViewData["Email"] = user == null? email : user.Email;
       ViewData["AppUserId"] = user == null? appUserId: user.Id;
       return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CompleteProfile(Customer customer)
    {
        Console.WriteLine($"Saving customer: {customer.Name}, {customer.AppUserId} {customer.Email}, {customer.Phone}");
        
        if (!ModelState.IsValid)
        {
            Console.WriteLine(string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return View(customer);
        }
        else
        {
            await _customerService.CreateCustomer(customer);
            return RedirectToAction("Index", "Home");
        }
    }
}