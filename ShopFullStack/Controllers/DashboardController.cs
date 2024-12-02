using Microsoft.AspNetCore.Mvc;
using ShopFullStack.Models;
using ShopFullStack.Services;

namespace ShopFullStack.Controllers;

public class DashboardController: Controller
{
    
    private readonly OrderService _orderService;
    private readonly ProductService _productService;
    private readonly CustomerService _customerService;

    public DashboardController(
        OrderService orderService, 
        ProductService productService,
        CustomerService customerService
        )
    {
        _orderService = orderService;
        _productService = productService;
        _customerService = customerService;
    }
    
    [HttpGet]
    public async Task<IActionResult> ProductDetailsAdmin(long id)
    {
        var response = await _productService.GetProductById(id);
        return View(response.Data);
    }
    
    
    public async Task<IActionResult> Dashboard()
    {
        var popularProducts = await _productService.GetMostPopularProducts();
        var unpaidOrders = await _orderService.GetAllUnpaidOrders();
        var returnedOrders = await _orderService.GetAllReturnedOrders();
        var products = await _productService.GetAllProducts();
        var expiredProducts = await _productService.GetExpiredProducts();
        var expiringSoonProducts = await _productService.GetProductsExpiringInNext24Hours();
        var customerShoppedLasWeek = await _customerService.GetAllCustomerShoppedLasWeek();

        var viewModel = new DashboardViewModel
        {
            PopularProducts = popularProducts,
            UnpaidOrders = unpaidOrders.Data ?? [],
            ReturnedOrders = returnedOrders.Data ?? [],
            Products = products,
            ExpiredProducts = expiredProducts.Data ?? [],
            ExpiringSoonProducts = expiringSoonProducts.Data ?? [],
            GetAllCustomerShoppedLasWeek = customerShoppedLasWeek.Data ?? [],
        };

        return View(viewModel);
    }
    
    public async Task<IActionResult> ProductSalesChart(DateFilter dateFilter)
    {
        var startDate = dateFilter.StartDate;
        var endDate = dateFilter.EndDate;

        var pieChartData = await GetPieChartData(startDate, endDate);

        return View(new ProductSalesViewModel
        {
            PieChartData = pieChartData,
            DateFilter = dateFilter
        });
    }
    
    private async Task<List<PieChartItem>> GetPieChartData(DateTime? startDate, DateTime? endDate)
    {
        var orders = await _orderService.AdminGetAllOrders();
    
       
        var filteredOrders = orders.Data == null? []: orders.Data 
            .Where(o => (!startDate.HasValue || o.CreatedAt >= startDate) &&
                        (!endDate.HasValue || o.CreatedAt <= endDate))
            .ToList();

   
        return filteredOrders
            .SelectMany(o => o.OrderItems)
            .GroupBy(oi => oi.ProductId)
            .Select(g => new PieChartItem
            {
                Label = g.First().Product?.Name ?? "Unknown Product",
                Value = g.Sum(oi => oi.Total)
            })
            .ToList();
    }
}