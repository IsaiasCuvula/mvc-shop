using Moq;
using ShopFullStack.Models;
using ShopFullStack.Repositories.Orders;
using ShopFullStack.Repositories.Product;
using ShopFullStack.Services;

namespace ShopApiTest.Services;

public class ProductServiceTest
{
    
    private readonly Mock<IProductRepository> _productRepository;
    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly ProductService _productService;
    
     
    public ProductServiceTest()
    {
        _productRepository = new Mock<IProductRepository>();
        _orderRepository = new Mock<IOrderRepository>();
        
        _productService = new ProductService(
            _productRepository.Object,
            _orderRepository.Object
        );

    }

    [Fact]
    public async Task CreateProduct_ShouldReturnNewProduct()
    {
        //Arrange
        const int productId = 5;
        DateTime dateTime = new DateTime(2025, 3, 1);
        var expectedProduct = new Product()
        {
            Id = productId,
            Name = "Fertilizer",
            Brand = "Green Thumb",
            Description = "Organic Fertilizer (5lb Bag)",
            Price = 14.99m,
            ExpirationDate = dateTime,
            Stock = 75,
            ProductNumber = 100005,
            ImageUrl = "fertilizer.jpg"
        };
        
        //Arrange
        _productRepository.Setup(
            repo => repo.AddAsync(It.IsAny<Product>())
        ).ReturnsAsync(expectedProduct);

        // Act
        var result = await _productService.CreateProduct(expectedProduct);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Status);
        Assert.Equal("Product created successfully", result.Message);

        var actualCustomer = result.Data;
        Assert.Equal(expectedProduct.Id, actualCustomer.Id);
        Assert.Equal(expectedProduct.ProductNumber, actualCustomer.ProductNumber);

        // Verify if the methods were called
        _productRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
    }
    
    [Fact]
    public async Task GetProductById_ShouldReturnProduct()
    {
        //Arrange
        const int productId = 5;
        DateTime dateTime = new DateTime(2025, 3, 1);
        var expectedProduct = new Product()
        {
            Id = productId,
            Name = "Fertilizer",
            Brand = "Green Thumb",
            Description = "Organic Fertilizer (5lb Bag)",
            Price = 14.99m,
            ExpirationDate = dateTime,
            Stock = 75,
            ProductNumber = 100005,
            ImageUrl = "fertilizer.jpg"
        };
        
        //Arrange
        _productRepository.Setup(
            repo => repo.GetByIdAsync(productId)
        ).ReturnsAsync(expectedProduct);

        // Act
        var result = await _productService.GetProductById(productId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Status);
        Assert.Equal("Product successfully retrieved", result.Message);

        var actualCustomer = result.Data;
        Assert.Equal(expectedProduct.Id, actualCustomer.Id);
        Assert.Equal(expectedProduct.ProductNumber, actualCustomer.ProductNumber);

        // Verify if the methods were called
        _productRepository.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
    }
    
    [Fact]
    public async Task GetProductById_ShouldNotReturnProduct()
    {
        //Arrange
        const int productIdNotInDb = 299;
        const int productId = 5;
        DateTime dateTime = new DateTime(2025, 3, 1);
        var expectedProduct = new Product()
        {
            Id = productId,
            Name = "Fertilizer",
            Brand = "Green Thumb",
            Description = "Organic Fertilizer (5lb Bag)",
            Price = 14.99m,
            ExpirationDate = dateTime,
            Stock = 75,
            ProductNumber = 100005,
            ImageUrl = "fertilizer.jpg"
        };
        
        //Arrange
        _productRepository.Setup(
            repo => repo.GetByIdAsync(productId)
        ).ReturnsAsync(expectedProduct);

        // Act
        var result = await _productService.GetProductById(productIdNotInDb);

        // Assert
        Assert.Null(result.Data);
        Assert.True(result.Status);
        Assert.Equal("Product not found", result.Message);

        var actualCustomer = result.Data;
        
        // Verify if the methods were called
        _productRepository.Verify(repo => repo.GetByIdAsync(productIdNotInDb), Times.Once);
    }
    
    
    [Fact]
    public async Task GetProducts_Returns_Correct_Number_Of_Products()
    {
        //Arrange
        var expectedProducts = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Headphones",
                Brand = "Acme Co.",
                Description = "Wireless Bluetooth Headphones",
                Price = 99.99m,
                ExpirationDate = new DateTime(2030, 12, 31),
                Stock = 50,
                ProductNumber = 100001,
                ImageUrl = "headphones.jpg"
            },
            new Product
            {
                Id = 2,
                Name = "Almond Milk",
                Brand = "Fresh Farms",
                Description = "Organic Almond Milk",
                Price = 3.49m,
                ExpirationDate = new DateTime(2024, 5, 15),
                Stock = 200,
                ProductNumber = 100002,
                ImageUrl = "almond_milk.jpg"
            },
            new Product
            {
                Id = 3,
                Brand = "Eco Clean",
                Name = "Dish Soap",
                Description = "Biodegradable Dish Soap",
                Price = 5.99m,
                ExpirationDate = new DateTime(2026, 8, 20),
                Stock = 120,
                ProductNumber = 100003,
                ImageUrl = "dish_soap.jpg"
            },
            new Product
            {
                Id = 4,
                Name = "TechLine",
                Brand = "TechLine",
                Description = "4K Ultra HD Smart TV",
                Price = 399.99m,
                ExpirationDate = new DateTime(2030, 12, 31),
                Stock = 10,
                ProductNumber = 100004,
                ImageUrl = "smart_tv.jpg"
            },
            new Product
            {
                Id = 5,
                Name = "Fertilizer",
                Brand = "Green Thumb",
                Description = "Organic Fertilizer (5lb Bag)",
                Price = 14.99m,
                ExpirationDate = new DateTime(2025, 3, 1),
                Stock = 75,
                ProductNumber = 100005,
                ImageUrl = "fertilizer.jpg"
            }
        };


        _productRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(expectedProducts);
        
        //Act
        var result = await  _productService.GetAllProducts();
        //Assert
        Assert.Equal(expectedProducts.Count, result.Count);
        
        // Verify if the methods were called
        _productRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}