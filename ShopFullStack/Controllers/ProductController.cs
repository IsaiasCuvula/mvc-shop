using Microsoft.AspNetCore.Mvc;
using ShopFullStack.Dtos;
using ShopFullStack.Models;
using ShopFullStack.Services;
namespace ShopFullStack.Controllers;

public class ProductController: Controller
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProducts();
        return View(products);
    }
    
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<Product>>>> GetAllProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }
    
    [HttpGet("most-popular")]
    public async Task<ActionResult<ApiResponse<List<Product>>>> GetMostPopularProducts()
    {
        var products = await _productService.GetMostPopularProducts();
        return Ok(products);
    } 
    
    [HttpGet("expire-in-3-months")]
    public async Task<ActionResult<ApiResponse<List<Product>>>> GetProductsExpiringInNext3Months()
    {
        var products = await _productService.GetProductsExpiringInNext3Months();
        return Ok(products);
    } 
    
    [HttpGet("expire-in-24-hours")]
    public async Task<ActionResult<ApiResponse<List<Product>>>> GetProductsExpiringInNext24Hours()
    {
        var products = await _productService.GetProductsExpiringInNext24Hours();
        return Ok(products);
    }
    
    [HttpGet("expired")]
    public async Task<ActionResult<ApiResponse<List<Product>>>> GetExpiredProducts()
    {
        var products = await _productService.GetExpiredProducts();
        return Ok(products);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Product>>> UpdateProduct(long id, ProductDto dto)
    {
        return Ok(await _productService.UpdateProduct(dto, id));
    }
    
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Product>>> CreateProduct(ProductDto dto)
    {
        return Ok(await _productService.CreateProduct(dto));
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Product>>> GetProductById(long id)
    {
        return Ok(await _productService.GetProductById(id));
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Product>>> DeleteProductById(long id)
    {
        return Ok(await _productService.DeleteProductById(id));
    }
}