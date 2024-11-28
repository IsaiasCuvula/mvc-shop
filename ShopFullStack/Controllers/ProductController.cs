using Microsoft.AspNetCore.Mvc;
using ShopFullStack.Dtos;
using ShopFullStack.Models;
using ShopFullStack.Services;
namespace ShopFullStack.Controllers;

public class ProductController: Controller
{
    private readonly ProductService _productService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(
        ProductService productService,
        IWebHostEnvironment webHostEnvironment
    )
    {
        _productService = productService;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _productService.GetAllProducts());
    }
    
    //When user click on submit form this method is called
    [HttpPost]
    public async Task<IActionResult> AddEdit(Product product)
    {
        ViewBag.Products = await _productService.GetAllProducts();
        
        try
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("****************************");
                Console.WriteLine(
                    $"Error: {ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage}"
                );
                Console.WriteLine("****************************");
                return View(new Product());
            }
            
            //Add product 
            if (product.Id == 0)
            {
                if (product.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }
                    product.ImageUrl = uniqueFileName;
                }
                await _productService.CreateProduct(product);
            }
            return RedirectToAction("Index", "Product");
        }
        catch (Exception e)
        {
            Console.WriteLine("****************************");
            Console.WriteLine($"Error: {e.Message}");
            Console.WriteLine("****************************");
            return View(new Product());
        }
    }
    
    //When user click on add new product on all product page 
    //this method is called
    [HttpGet]
    public async Task<IActionResult> AddEdit(int id)
    {
        ViewBag.Products = await _productService.GetAllProducts();
    
        if (id == 0)
        {
            ViewBag.Operation = "Add";
            return View(new Product());
        }else
        {
            ApiResponse<Product> response = await _productService.GetProductById(id);
            ViewBag.Operation = "Edit";
            return View(response.Data);
        }
    }
    // [HttpGet]
    // public async Task<ActionResult<ApiResponse<List<Product>>>> GetAllProducts()
    // {
    //     var products = await _productService.GetAllProducts();
    //     return Ok(products);
    // }
    //
    // [HttpGet("most-popular")]
    // public async Task<ActionResult<ApiResponse<List<Product>>>> GetMostPopularProducts()
    // {
    //     var products = await _productService.GetMostPopularProducts();
    //     return Ok(products);
    // } 
    //
    // [HttpGet("expire-in-3-months")]
    // public async Task<ActionResult<ApiResponse<List<Product>>>> GetProductsExpiringInNext3Months()
    // {
    //     var products = await _productService.GetProductsExpiringInNext3Months();
    //     return Ok(products);
    // } 
    //
    // [HttpGet("expire-in-24-hours")]
    // public async Task<ActionResult<ApiResponse<List<Product>>>> GetProductsExpiringInNext24Hours()
    // {
    //     var products = await _productService.GetProductsExpiringInNext24Hours();
    //     return Ok(products);
    // }
    //
    // [HttpGet("expired")]
    // public async Task<ActionResult<ApiResponse<List<Product>>>> GetExpiredProducts()
    // {
    //     var products = await _productService.GetExpiredProducts();
    //     return Ok(products);
    // }
    //
    // [HttpPut("{id}")]
    // public async Task<ActionResult<ApiResponse<Product>>> UpdateProduct(long id, ProductDto dto)
    // {
    //     return Ok(await _productService.UpdateProduct(dto, id));
    // }
    //
    // [HttpPost]
    // public async Task<ActionResult<ApiResponse<Product>>> CreateProduct(ProductDto dto)
    // {
    //     return Ok(await _productService.CreateProduct(dto));
    // }
    //
    // [HttpGet("{id}")]
    // public async Task<ActionResult<ApiResponse<Product>>> GetProductById(long id)
    // {
    //     return Ok(await _productService.GetProductById(id));
    // }
    //
    // [HttpDelete("{id}")]
    // public async Task<ActionResult<ApiResponse<Product>>> DeleteProductById(long id)
    // {
    //     return Ok(await _productService.DeleteProductById(id));
    // }
}