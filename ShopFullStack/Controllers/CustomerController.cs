using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopFullStack.Models;
using ShopFullStack.Services;

namespace ShopFullStack.Controllers;

public class CustomerController: Controller
{
    
    private readonly CustomerService _customerService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public CustomerController(CustomerService customerService, 
        UserManager<IdentityUser> userManager,  SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _customerService = customerService;
    }
    
    //Delete
    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (user == null)
            {
                return Unauthorized();
            }
            
            await _customerService.DeleteCustomerById(id);
            await _userManager.DeleteAsync(user);
            
            // Log out the user
            await _signInManager.SignOutAsync();
            
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            Console.WriteLine("****************************");
            Console.WriteLine($"Error: {e.Message}");
            Console.WriteLine("****************************");
            
            return RedirectToAction("Index", "Home");
        }
    } 
    
    
    [HttpGet]
    public async Task<IActionResult> UpdateCustomer()
    {   
        var user = _userManager.GetUserAsync(User).Result;
        var email = user == null?  HttpContext.Session.GetString("Email"): user.Email;
        var appUserId = user == null? HttpContext.Session.GetString("AppUserId") : user.Id;
        ViewData["Email"] = email;
        ViewData["AppUserId"] = appUserId;
        var result = await _customerService
            .GetCustomerByEmail(email);

        if (result.Data == null)
        {
            var customerToBeCreated = new Customer
            {
                Email = email,
                AppUserId = appUserId,
                Id = 0,
                Name =  string.Empty,
                City = string.Empty,
                Address = string.Empty,
                Phone = string.Empty,
            };
            return View(customerToBeCreated);
        }

        return View(result.Data);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCustomer(Customer customer)
    {
        var user = _userManager.GetUserAsync(User).Result;
        var email = user == null?  HttpContext.Session.GetString("Email"): user.Email;
        var appUserId = user == null? HttpContext.Session.GetString("AppUserId") : user.Id;
        ViewData["Email"] = email;
        ViewData["AppUserId"] = appUserId;

        if (!ModelState.IsValid)
        {
            Console.WriteLine(
                string.Join(", ", 
                    ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                )
            );
            return View();
        }
        else
        {
            //If user does not have all info, create a customer
            var response = customer.Id == 0 ? 
                await _customerService.CreateCustomer(customer) : 
                await _customerService.UpdateCustomer(customer);
            
            return View(response.Data);
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