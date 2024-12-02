using ShopFullStack.Models;
using ShopFullStack.Repositories.Orders;
using ShopFullStack.Repositories.Product;
using ShopFullStack.Utilities;

namespace ShopFullStack.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;

    public ProductService(
        IProductRepository productRepository, 
        IOrderRepository orderRepository
    )
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }
    
    public async Task<ApiResponse<Product>> UpdateProduct(Product product)
    {
        ApiResponse<Product> response = new ApiResponse<Product>();
        try
        {
            var oldProduct = await _productRepository.GetByIdAsync(product.Id);
            if (oldProduct == null)
            {
                response.Message = "Product not found";
                return response;
            }

            if (product.ImageUrl != null)
            {
                oldProduct.ImageUrl = product.ImageUrl;
            }
            
            oldProduct.Brand = product.Brand;
            oldProduct.Description = product.Description;
            oldProduct.ExpirationDate = product.ExpirationDate.ToUniversalTime();
            oldProduct.Name = product.Name;
            oldProduct.Price = product.Price;
            oldProduct.ProductNumber = product.ProductNumber;
            oldProduct.Stock = product.Stock;
            
            var updatedProduct = await _productRepository.UpdateAsync(oldProduct);
           
            response.Data = updatedProduct;
            response.Message = "Product updated successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to updated product with id: {product.Id} - {e}");
            return response;
        }
    }

    
    public async Task<ApiResponse<Product>>  CreateProduct(Product product)
    {
        ApiResponse<Product> response = new ApiResponse<Product>();
        try
        {
            product.ProductNumber = AppHelpers.GenerateRandomNumber();
            product.ExpirationDate = product.ExpirationDate.ToUniversalTime();
            var savedProduct = await _productRepository.AddAsync(product);
           
            response.Data = savedProduct;
            response.Message = "Product created successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to create product - {e}");
            return response;
        }
    }

    public async Task<List<Product>> GetMostPopularProducts()
    {
        Dictionary<long, int> productsCount = new Dictionary<long, int>();
        List<Product> popularProducts = new List<Product>();
    
        try
        {
            var orders = await _orderRepository.GetMostPopularAsync();
            foreach (var order in orders)
            {
                Console.WriteLine($"OrderItems: {order.OrderItems.Count}");
               foreach (var item in order.OrderItems)
               {
                   Console.WriteLine($"ProductId: {item.ProductId}");
                  if (productsCount.ContainsKey(item.ProductId)) 
                  {
                       productsCount[item.ProductId] += item.Quantity;
                  }
                  else
                  {
                      productsCount[item.ProductId] = item.Quantity;
                  }
               }
            }
           
            var sortedProducts = productsCount
                .OrderByDescending(x => x.Value);
           
            foreach (var dic in sortedProducts)
            {
                Console.WriteLine($"dic: {dic.Key} - {dic.Value}");
                var product = await _productRepository.GetByIdAsync(dic.Key);
                
                Console.WriteLine($"product: {product}");
                
                if (product != null)
                {                
                    popularProducts.Add(product);
                }
            }
            return popularProducts;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get the most popular products: {e}");
            return [];
        }
    }
    
    public async Task<ApiResponse<List<Product>>> GetProductsExpiringInNext3Months()
    {
        ApiResponse<List<Product>> response = new ApiResponse<List<Product>>();
        try
        {
            var products = await _productRepository.GetProductsExpiringInNext3MonthsAsync();
            response.Data = products;
            response.Message = products.Count == 0 ? "There is no product that will expire in next 3 months":"All products that will expire in next 3 months successfully retrieved";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get all products will expire in the next 3 months: {e}");
            return response;
        }
    }
    
    public async Task<ApiResponse<List<Product>>> GetProductsExpiringInNext24Hours()
    {
        ApiResponse<List<Product>> response = new ApiResponse<List<Product>>();
        try
        {
            var products = await _productRepository.GetProductsExpiringInNext24HoursAsync();
            response.Data = products;
            response.Message = products.Count == 0 ? "There is no product that will expire in next 24h":"All products that will expire in next 24h successfully retrieved";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get all products that will expire in the next 24h: {e}");
            return response;
        }
    }
    
    public async Task<ApiResponse<List<Product>>> GetExpiredProducts()
    {
        ApiResponse<List<Product>> response = new ApiResponse<List<Product>>();
        try
        {
            var products = await _productRepository.GetExpiredProductsAsync();
            response.Data = products;
            response.Message = products.Count == 0 ? "There is no expired product yet":"All Expired products successfully retrieved";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get all expired products: {e}");
            return response;
        }
    }
   
  
    public async Task<ApiResponse<Product>>  DeleteProductById(long id)
    {
        ApiResponse<Product> response = new ApiResponse<Product>();
        try
        {
            var product = await _productRepository.GetByIdAsync(id);
            
            if (product == null)
            {
                response.Message = "Product not found";
                return response;
            }
           
            await _productRepository.DeleteAsync(product);
           
            response.Data = null;
            response.Message = "Product deleted successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get product with id: {id} - {e}");
            return response;
        }
    }
    
    public async Task<ApiResponse<Product>> GetProductById(long id)
    {
        ApiResponse<Product> response = new ApiResponse<Product>();
        try
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                response.Message = "Product not found";
                return response;
            }
            response.Data = product;
            response.Message = "Product successfully retrieved";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get product with id: {id} - {e}");
            return response;
        }
    }

    public async Task<List<Product>> GetAllProducts()
    {
        try
        {
            return await _productRepository.GetAllAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get all products: {e}");
            return [];
        }
    }
}