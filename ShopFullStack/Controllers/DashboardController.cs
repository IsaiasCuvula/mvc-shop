using Microsoft.AspNetCore.Mvc;
using ShopFullStack.Models;
using ShopFullStack.Services;

namespace ShopFullStack.Controllers;

public class DashboardController: Controller
{
    
    private readonly OrderService _orderService;
    private readonly ProductService _productService;

    public DashboardController(OrderService orderService, ProductService productService)
    {
        _orderService = orderService;
        _productService = productService;
    }
    
    
    public async Task<IActionResult> Dashboard()
    {
        var popularProducts = await _productService.GetMostPopularProducts();
        var unpaidOrders = await _orderService.GetAllUnpaidOrders();
        var returnedOrders = await _orderService.GetAllReturnedOrders();
        var products = await _productService.GetAllProducts();
        var expiredProducts = await _productService.GetExpiredProducts();
        var expiringSoonProducts = await _productService.GetProductsExpiringInNext24Hours();

        var pieChartData = await GetPieChartData();
        
        
        Console.WriteLine("*********************************");
        Console.WriteLine($"popularProducts: {popularProducts.Count}");
        Console.WriteLine($"unpaidOrders: {unpaidOrders.Message}");
        Console.WriteLine($"returnedOrders: {returnedOrders.Message}");
        Console.WriteLine($"products: {products.Count}");
        Console.WriteLine($"expiredProducts: {expiredProducts.Message}");
        Console.WriteLine($"expiringSoonProducts: {expiringSoonProducts.Message}");
        Console.WriteLine("*********************************");

        var viewModel = new DashboardViewModel
        {
            PopularProducts = popularProducts,
            UnpaidOrders = unpaidOrders.Data ?? [],
            ReturnedOrders = returnedOrders.Data ?? [],
            Products = products,
            ExpiredProducts = expiredProducts.Data ?? [],
            ExpiringSoonProducts = expiringSoonProducts.Data ?? [],
            PieChartData = pieChartData
        };

        return View(viewModel);
    }
    
    
    private async Task<List<PieChartItem>> GetPieChartData()
    {
        var orders = await _orderService.AdminGetAllOrders(); 
        return orders.Data == null ? [] : orders.Data.GroupBy(o => o.Id)
            .Select(g => new PieChartItem
            {
                Label = $"Order To be implemented",
                Value = g.Sum(o => o.Total)
            })
            .ToList();
    }
}