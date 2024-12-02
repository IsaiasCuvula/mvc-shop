using ShopFullStack.Models;
using ShopFullStack.Repositories;
using ShopFullStack.Repositories.Orders;
using ShopFullStack.Repositories.Product;

namespace ShopFullStack.Services;

public class OrderService
{
     private readonly IOrderRepository _orderRepository;
     private readonly IProductRepository _productRepository;
     private readonly ICustomerRepository _customerRepository;

    public OrderService(
        IOrderRepository orderRepository, 
        IProductRepository productRepository, 
        ICustomerRepository customerRepository
   )
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }
    
    public async Task<ApiResponse<Order>> GetOrderById(long id)
    {
        ApiResponse<Order> response = new ApiResponse<Order>();
        try
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                response.Message = "Order not found";
                return response;
            }
            response.Data = order;
            response.Message = "Order successfully retrieved";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get order with id: {id} - {e}");
            return response;
        }
    }
    
    public async Task<ApiResponse<List<Order>>> GetAllOrders(long customerId)
    {
        ApiResponse<List<Order>> response = new ApiResponse<List<Order>>();
        try
        {
          var orders = await _orderRepository.GetAllAsync(customerId);
          response.Data = orders;
          response.Message = orders.Count == 0 ? "There is no order created yet":"Orders successfully retrieved";
          return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get all orders: {e}");
            return response;
        }
    }
    
    public async Task<ApiResponse<Order>>  CreateOrder(Order order)
    {
        var response = new ApiResponse<Order>();
        try
        {
            response.Data = await _orderRepository.AddAsync(order);
            response.Message = "Order created successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to create order - {e}");
            return response;
        }
    }
    
    public async Task<ApiResponse<List<Order>>> GetAllUnpaidOrders()
    {
        ApiResponse<List<Order>> response = new ApiResponse<List<Order>>();
        try
        {
            var unpaidOrders = await _orderRepository.GetAllUnpaidOrdersAsync();
            response.Data = unpaidOrders;
            response.Message = unpaidOrders.Count == 0 ? "There is no unpaid order":"All unpaid orders successfully retrieved";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get all unpaid orders: {e}");
            return response;
        }
    }

    // public async Task<ApiResponse<List<Order>>> GetAllReturnedOrders()
    // {
    //     ApiResponse<List<Order>> response = new ApiResponse<List<Order>>();
    //     try
    //     {
    //         var returnedOrders = await _orderRepository.GetAllReturnedOrdersAsync();
    //         response.Data = returnedOrders;
    //         response.Message = returnedOrders.Count == 0 ? "There is no returned order yet":"All returned orders successfully retrieved";
    //         return response;
    //     }
    //     catch (Exception e)
    //     {
    //         response.Message = e.Message;
    //         response.Status = false;
    //         Console.WriteLine($"Failed to get all returned orders: {e}");
    //         return response;
    //     }
    // }
    //
    
    // private async Task<List<Order>> CheckoutOrder(List<Order> orders, decimal deliveryPrice)
    // {
    //     decimal totalToPay = orders.Sum(o => o.Total) + deliveryPrice;
    //     //Order number (orderId) save it into stripe
    //     //
    //     return await UpdateOrderAfterPayment(orders);
    // }
    //
    // private async Task<List<Order>> UpdateOrderAfterPayment(List<Order> orders)
    // {
    //     List<Order> updatedOrders = new List<Order>();
    //     foreach (var order in orders)
    //     {
    //         order.PaymentDate = DateTime.Now.ToUniversalTime();
    //         order.PaymentStatus = PaymentStatus.Paid;
    //         var updatedOrder = await _orderRepository.UpdateAsync(order);
    //         updatedOrders.Add(updatedOrder);
    //     }
    //     return updatedOrders;
    // }
    //
    // private async Task<decimal> GetTotalByProduct(OrderDto dto)
    // {
    //     var product = await _productRepository.GetByNumberAsync(dto.ProductNumber);
    //     if (product == null){return 0;}
    //     return product.Price * dto.Quantity;
    // }
    //
    // public async Task<ApiResponse<Order>> UpdateOrder(OrderDto dto, long orderId)
    // {
    //     ApiResponse<Order> response = new ApiResponse<Order>();
    //     try
    //     {
    //         var order = await _orderRepository.GetByIdAsync(orderId);
    //         if (order == null)
    //         {
    //             response.Message = "Order not found";
    //             return response;
    //         }
    //         Console.WriteLine("******************************************");
    //         Console.WriteLine($"Old Total {order.Total}");
    //         Console.WriteLine($"New Total {await GetTotalByProduct(dto)}");
    //         Console.WriteLine("******************************************");
    //         order.Total = await GetTotalByProduct(dto);
    //         order.Quantity = dto.Quantity;
    //         order.CustomerNumber = dto.CustomerNumber;
    //         order.ProductNumber = dto.ProductNumber;
    //         order.ReturnStatus = dto.ReturnStatus;
    //     
    //         var updatedOrder = await _orderRepository.UpdateAsync(order);
    //        
    //         response.Data = updatedOrder;
    //         response.Message = "Order updated successfully";
    //         return response;
    //     }
    //     catch (Exception e)
    //     {
    //         response.Message = e.Message;
    //         response.Status = false;
    //         Console.WriteLine($"Failed to updated order with id: {orderId} - {e}");
    //         return response;
    //     }
    // }
    //
    // public async Task<ApiResponse<Order>>  DeleteOrderById(long id)
    // {
    //     ApiResponse<Order> response = new ApiResponse<Order>();
    //     try
    //     {
    //         var order = await _orderRepository.GetByIdAsync(id);
    //         
    //         if (order == null)
    //         {
    //             response.Message = "Order not found";
    //             return response;
    //         }
    //        
    //         await _orderRepository.DeleteAsync(order);
    //        
    //         response.Data = null;
    //         response.Message = "Order deleted successfully";
    //         return response;
    //     }
    //     catch (Exception e)
    //     {
    //         response.Message = e.Message;
    //         response.Status = false;
    //         Console.WriteLine($"Failed to get order with id: {id} - {e}");
    //         return response;
    //     }
    // }
    //
    
}