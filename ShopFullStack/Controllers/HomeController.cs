using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShopFullStack.Models;
using ShopFullStack.Services;

namespace ShopFullStack.Controllers;

public class HomeController: Controller
{
    private readonly ProductService _productService;

    public HomeController(ProductService productService, ILogger<HomeController> logger)
    {
        _productService = productService;
    }
    

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetMostPopularProducts();
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}