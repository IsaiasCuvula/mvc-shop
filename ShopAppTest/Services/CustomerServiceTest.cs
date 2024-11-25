using Moq;
using ShopApi.Dtos;
using ShopApi.Models;
using ShopApi.Repositories;
using ShopApi.Repositories.Orders;
using ShopApi.Services;

namespace ShopApiTest;

public class CustomerServiceTest
{
    private readonly CustomerService _customerService;
    private readonly Mock<ICustomerRepository> _customerRepository;
    private readonly Mock<IOrderRepository> _orderRepository;

    
    public CustomerServiceTest()
    {
        _customerRepository = new Mock<ICustomerRepository>();
        _orderRepository = new Mock<IOrderRepository>();
        
        _customerService =new CustomerService(
            _customerRepository.Object,
            _orderRepository.Object
       );

    }

    [Fact]
    public async Task CreateCustomer_ValidCustomer_ShouldReturnNewCustomer()
    {
        //Arrange
        const int customerId = 1;
        var expectedCustomer = new Customer
        {
            Id = customerId,
            Name = "John Doe",
            IdCardNumber = 123456789,
            City = "New York",
            Address ="New York",
            Phone = "+1-555-1234",
            Email = "johndoe@example.com",
            CustomerNumber = 987654321,
        };
        var dto =new CustomerDto(
            "John Doe",
             123456789,
            "New York",
             "New York",
             "+1-555-1234",
             "johndoe@example.com"
            );
        
        //Arrange
        _customerRepository.Setup(
            repo => repo.AddAsync(It.IsAny<Customer>())
        ).ReturnsAsync(expectedCustomer);

        // Act
        var result = await _customerService.CreateCustomer(dto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Status);
        Assert.Equal("Customer created successfully", result.Message);

        var actualCustomer = result.Data;
        Assert.Equal(expectedCustomer.Id, actualCustomer.Id);
        Assert.Equal(expectedCustomer.Name, actualCustomer.Name);
        Assert.Equal(expectedCustomer.Email, actualCustomer.Email);
        Assert.Equal(expectedCustomer.Address, actualCustomer.Address);
        Assert.Equal(expectedCustomer.City, actualCustomer.City);
        Assert.Equal(expectedCustomer.CustomerNumber, actualCustomer.CustomerNumber);

        // Verify if the methods were called
        _customerRepository.Verify(repo => repo.AddAsync(It.IsAny<Customer>()), Times.Once);
    }
    
    [Fact]
    public async Task DeleteCustomerById_ShouldReturnNullAfterDelete()
    {
        //Arrange
        const int customerId = 1;
        var expectedCustomer = new Customer
        {
            Id = customerId,
            Name = "John Doe",
            IdCardNumber = 123456789,
            City = "New York",
            Address ="New York",
            Phone = "+1-555-1234",
            Email = "johndoe@example.com",
            CustomerNumber = 987654321,
        };
        
        //Arrange
        _customerRepository.Setup(
            repo => repo.GetByIdAsync(customerId)
        ).ReturnsAsync(expectedCustomer);
    
        //Act
        var result = await _customerService.DeleteCustomerById(customerId);
    
        //Assert
        var expectedMsg = "Customer deleted successfully";
     
        Assert.Equal(result.Message, expectedMsg);
        Assert.Null(result.Data);
    }
    
    [Fact]
    public async Task DeleteCustomerById_ShouldReturnNullWhenCustomerNotFound()
    {
        //Arrange
        const int customerId = 1;
        var expectedCustomer = new Customer
        {
            Id = customerId,
            Name = "John Doe",
            IdCardNumber = 123456789,
            City = "New York",
            Address ="New York",
            Phone = "+1-555-1234",
            Email = "johndoe@example.com",
            CustomerNumber = 987654321,
        };
        
        //Arrange
        _customerRepository.Setup(
            repo => repo.GetByIdAsync(customerId)
        ).ReturnsAsync(expectedCustomer);
    
        //Act
        var result = await _customerService.DeleteCustomerById(02);
    
        //Assert
        var expectedMsg = "Customer not found";
     
        Assert.Equal(result.Message, expectedMsg);
        Assert.Null(result.Data);
    }
    
    
     [Fact]
     public async Task GetUserByIdAsync_ShouldReturnUser()
     {
         //Arrange
         const int customerId = 1;
         var expectedCustomer = new Customer
         {
             Id = customerId,
             Name = "John Doe",
             IdCardNumber = 123456789,
             City = "New York",
             Address = "123 Elm Street",
             Phone = "+1-555-1234",
             Email = "johndoe@example.com",
             CustomerNumber = 987654321,
         };

         _customerRepository.Setup(repo => repo.GetByIdAsync(customerId))
             .ReturnsAsync(expectedCustomer);
        
         //Act
         var result = await _customerService.GetCustomerById(customerId);
        
         //Assert
         Assert.NotNull(result);
         Assert.Equal(expectedCustomer, result.Data);
    }
     
     [Fact]
     public async Task GetUserByIdAsync_ShouldNotReturnUser()
     {
         //Arrange
         const int customerId = 1;

         //Act
         var result = await _customerService.GetCustomerById(customerId);
        
         //Assert
         var expectedMSg = "Customer not found";
         
         Assert.NotNull(result);
         Assert.Equal(expectedMSg, result.Message);
     }
    
    [Fact]
    public async Task GetCustomers_Returns_Correct_Number_Of_Customers()
    {
        //Arrange
        var expectedCustomers = new List<Customer>
        {
            new Customer
            {
                Id = 1,
                Name = "John Doe",
                IdCardNumber = 123456789,
                City = "New York",
                Address = "123 Elm Street",
                Phone = "+1-555-1234",
                Email = "johndoe@example.com",
                CustomerNumber = 987654321,
            },
            new Customer
            {
                Id = 2,
                Name = "Jane Smith",
                IdCardNumber = 987654321,
                City = "Los Angeles",
                Address = "456 Maple Avenue",
                Phone = "+1-555-5678",
                Email = "janesmith@example.com",
                CustomerNumber = 123456789,
            },
            new Customer
            {
                Id = 3,
                Name = "Alice Johnson",
                IdCardNumber = 567890123,
                City = "Chicago",
                Address = "789 Oak Drive",
                Phone = "+1-555-9012",
                Email = "alicejohnson@example.com",
                CustomerNumber = 112233445,
            },
            new Customer
            {
                Id = 4,
                Name = "Bob Brown",
                IdCardNumber = 345678901,
                City = "Houston",
                Address = "321 Pine Lane",
                Phone = "+1-555-3456",
                Email = "bobbrown@example.com",
                CustomerNumber = 556677889,
            },
            new Customer
            {
                Id = 5,
                Name = "Eve Davis",
                IdCardNumber = 234567890,
                City = "Phoenix",
                Address = "654 Cedar Court",
                Phone = "+1-555-7890",
                Email = "evedavis@example.com",
                CustomerNumber = 998877665,
            }
        };

        _customerRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(expectedCustomers);
        //Act
        var result = await  _customerService.GetAllCustomers();
        //Assert
        Assert.Equal(expectedCustomers.Count, result.Data.Count);
    }
}