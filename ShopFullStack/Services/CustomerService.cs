using ShopFullStack.Dtos;
using ShopFullStack.Mappers;
using ShopFullStack.Models;
using ShopFullStack.Repositories;
using ShopFullStack.Repositories.Orders;
using ShopFullStack.Utilities;

namespace ShopFullStack.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;

    public CustomerService(
        ICustomerRepository customerRepository, 
        IOrderRepository orderRepository
        )
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
    }
    
    public async Task<ApiResponse<Customer>> GetTopCustomerByTurnover()
    {
        ApiResponse<Customer> response = new ApiResponse<Customer>();
        Dictionary<long, decimal> customersByTurnover = new Dictionary<long, decimal>();
        try
        {
            var orders = await _orderRepository.GetAllAsync();

            foreach (var order in orders)
            {
                if (customersByTurnover.ContainsKey(order.CustomerNumber))
                {
                    customersByTurnover[order.CustomerNumber] += order.Total;
                }
                else
                {
                    customersByTurnover[order.CustomerNumber] = order.Total;
                }
            }
            var sortedProducts = customersByTurnover.OrderByDescending(x => x.Value).ToList();

            if (sortedProducts.Count <= 0)
            {
                response.Message = "There is no top customer yet";
                return response;
            }
            
            var topCustomerDic = sortedProducts.First();
            var topCustomer = await _customerRepository.GetByNumberAsync(topCustomerDic.Key);

            if (topCustomer == null)
            {
                response.Message = "There is no top customer yet";
                return response;
            }
            response.Data = topCustomer;
            response.Message = "Top customer fetched successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get top customer: {e}");
            return response;
        }
    }

    
    public async Task<ApiResponse<List<Customer>>> GetAllCustomerShoppedLasWeek()
    {
        ApiResponse<List<Customer>> response = new ApiResponse<List<Customer>>();
        try
        {
            var customers = await _customerRepository.GetAllCustomerShoppedLasWeekAsync();
            response.Data = customers;
            response.Message = customers.Count == 0 ? "There is no customer who shopped last week yet":"Customers who shopped last week successfully retrieved";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get all customers who shopped last week: {e}");
            return response;
        }
    }
    
    public async Task<ApiResponse<Customer>> UpdateCustomer(Customer customer, long customerId)
    {
        ApiResponse<Customer> response = new ApiResponse<Customer>();
        try
        {
            var oldCustomer = await _customerRepository.GetByIdAsync(customerId);
            if (oldCustomer == null)
            {
                response.Message = "Customer not found";
                return response;
            }
            
            oldCustomer.Name = customer.Name;
            oldCustomer.Email = customer.Email;
            oldCustomer.Phone = customer.Phone;
            oldCustomer.Address = customer.Address;
            oldCustomer.City = customer.City;
            oldCustomer.IdCardNumber = customer.IdCardNumber;
            
            
            var updatedCustomer = await _customerRepository.UpdateAsync(oldCustomer);
           
            response.Data = updatedCustomer;
            response.Message = "Customer updated successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to updated customer with id: {customerId} - {e}");
            return response;
        }
    }
    
    public async Task<ApiResponse<Customer>>  CreateCustomer(Customer customer)
    {
        ApiResponse<Customer> response = new ApiResponse<Customer>();
        try
        {
            customer.CustomerNumber = AppHelpers.GenerateRandomNumber();
            var savedCustomer = await _customerRepository.AddAsync(customer);
           
            response.Data = savedCustomer;
            response.Message = "Customer created successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to create customer - {e}");
            return response;
        }
    }
  
    public async Task<ApiResponse<Customer>>  DeleteCustomerById(long id)
    {
        ApiResponse<Customer> response = new ApiResponse<Customer>();
        try
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            
            if (customer == null)
            {
                response.Message = "Customer not found";
                return response;
            }
           
            await _customerRepository.DeleteAsync(customer);
           
            response.Data = null;
            response.Message = "Customer deleted successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get customer with id: {id} - {e}");
            return response;
        }
    }
    
    public async Task<ApiResponse<Customer>> GetCustomerById(long id)
    {
        ApiResponse<Customer> response = new ApiResponse<Customer>();
        try
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                response.Message = "Customer not found";
                return response;
            }
            response.Data = customer;
            response.Message = "Customer successfully retrieved";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get customer with id: {id} - {e}");
            return response;
        }
    }

    public async Task<ApiResponse<List<Customer>>> GetAllCustomers()
    {
        ApiResponse<List<Customer>> response = new ApiResponse<List<Customer>>();
        try
        {
          var customers = await _customerRepository.GetAllAsync();
          response.Data = customers;
          response.Message = customers.Count == 0 ? "There is no customer created yet":"Customers successfully retrieved";
          return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get all customers: {e}");
            return response;
        }
    }
}