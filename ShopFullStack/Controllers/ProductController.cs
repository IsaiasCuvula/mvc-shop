using Microsoft.AspNetCore.Mvc;
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

    public async Task<IActionResult> ProductsPage()
    {
        return View(await _productService.GetAllProducts());
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(long id)
    {
        ViewBag.Products = await _productService.GetAllProducts();
       
        var response = await _productService.GetProductById(id);
        ViewBag.Operation = "Edit";
        return View(response.Data);
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await _productService.DeleteProductById(id);
            return RedirectToAction("ProductsPage");
        }
        catch (Exception e)
        {
            return RedirectToAction("ProductsPage");
        }
    } 
    
    //When user click on submit form this method is called
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddEdit(Product product)
    {
        ViewBag.Products = await _productService.GetAllProducts();
        
        try
        {
            if (!ModelState.IsValid)
            {
                return View(new Product());
            }
            
            if (product.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(fileStream);
                }
                product.ImageUrl = uniqueFileName;
            }
            
            //Add product 
            if (product.Id == 0)
            {
                await _productService.CreateProduct(product);
            }
            else
            {
                // Update product
                await _productService.UpdateProduct(product);
            }
            
            return RedirectToAction("ProductsPage", "Product");
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
    //this method is called if id != 0 get product data to be updated
    [HttpGet]
    public async Task<IActionResult> AddEdit(long id)
    {
        ViewBag.Products = await _productService.GetAllProducts();
    
        if (id == 0)
        {
            ViewBag.Operation = "Add";
            return View(new Product());
        }else
        {
            var response = await _productService.GetProductById(id);
            ViewBag.Operation = "Edit";
            return View(response.Data);
        }
    }
}