using ShopApi.Dtos;
using ShopApi.Mappers;
using ShopApi.Models;
using ShopApi.Repositories.Orders;
using ShopApi.Repositories.Product;

namespace ShopApi.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;

    public ProductService(IProductRepository productRepository, IOrderRepository orderRepository)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    public async Task<ApiResponse<List<Product>>> GetMostPopularProducts()
    {
        ApiResponse<List<Product>> response = new ApiResponse<List<Product>>();
        Dictionary<long, int> productsCount = new Dictionary<long, int>();
        List<Product> popularProducts = new List<Product>();

        try
        {
            var orders = await _orderRepository.GetAllAsync();

            foreach (var order in orders)
            {
                if (productsCount.ContainsKey(order.ProductNumber))
                {
                    productsCount[order.ProductNumber] += order.Quantity;
                }
                else
                {
                    productsCount[order.ProductNumber] = order.Quantity;
                }
            }
            var sortedProducts = productsCount.OrderByDescending(x => x.Value);
            foreach (var dic in sortedProducts)
            {
                var product = await _productRepository.GetByNumberAsync(dic.Key);
                if (product != null)
                {                
                    popularProducts.Add(product);
                }
            }
            response.Data = popularProducts;
            response.Message = popularProducts.Count == 0 ? "There is no most popular product yet":"All most popular products successfully retrieved";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get the most popular products: {e}");
            return response;
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
    
    public async Task<ApiResponse<Product>> UpdateProduct(ProductDto dto, long productId)
    {
        ApiResponse<Product> response = new ApiResponse<Product>();
        try
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                response.Message = "Product not found";
                return response;
            }
            
            product.Brand = dto.Brand;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Image = dto.Image;
            product.Quantity = dto.Quantity;
            product.ExpirationDate = dto.ExpirationDate;
            
            var updatedProduct = await _productRepository.UpdateAsync(product);
           
            response.Data = updatedProduct;
            response.Message = "Product updated successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to updated product with id: {productId} - {e}");
            return response;
        }
    }
    
    public async Task<ApiResponse<Product>>  CreateProduct(ProductDto dto)
    {
        ApiResponse<Product> response = new ApiResponse<Product>();
        try
        {
           Product product = ProductMapper.MapToEntity(dto);
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

    public async Task<ApiResponse<List<Product>>> GetAllProducts()
    {
        ApiResponse<List<Product>> response = new ApiResponse<List<Product>>();
        try
        {
          var products = await _productRepository.GetAllAsync();
          response.Data = products;
          response.Message = products.Count == 0 ? "There is no product created yet":"Products successfully retrieved";
          return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get all products: {e}");
            return response;
        }
    }
}