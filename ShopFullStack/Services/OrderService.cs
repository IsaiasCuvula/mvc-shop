using ShopFullStack.Models;
using ShopFullStack.Repositories;
using ShopFullStack.Repositories.Orders;
using ShopFullStack.Repositories.Product;
using ShopFullStack.Utilities;

namespace ShopFullStack.Services;

public class OrderService
{
     private readonly IOrderRepository _orderRepository;

    public OrderService( IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<ApiResponse<Order>> GetOrderById(long id, long customerId)
    {
        var response = new ApiResponse<Order>();
        try
        {
            var order = await _orderRepository.GetByIdAsync(id, customerId);
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
        var response = new ApiResponse<List<Order>>();
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
    
    public async Task<ApiResponse<Order>> AdminGetOrderById(long orderId)
    {
        var response = new ApiResponse<Order>();
        try
        {
            var order = await _orderRepository.AdminGetByIdAsync(orderId);
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
            Console.WriteLine($"Failed to get order with id: {orderId} - {e}");
            return response;
        }
    }
    
    public async Task<ApiResponse<List<Order>>> AdminGetAllOrders()
    {
        var response = new ApiResponse<List<Order>>();
        try
        {
          var orders = await _orderRepository.AdminGetAllAsync();
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
        var response = new ApiResponse<List<Order>>();
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

    public async Task<ApiResponse<List<Order>>> GetAllReturnedOrders()
    {
       var response = new ApiResponse<List<Order>>();
        try
        {
            var returnedOrders = await _orderRepository.GetAllReturnedOrdersAsync();
            response.Data = returnedOrders;
            response.Message = returnedOrders.Count == 0 ? "There is no returned order yet":"All returned orders successfully retrieved";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to get all returned orders: {e}");
            return response;
        }
    }
    
    
    public async Task<ApiResponse<Order>> UpdateOrder(Order order)
    {
        var response = new ApiResponse<Order>();
        try
        {
            // var oldOrder = await _orderRepository.GetByIdAsync(order.Id);
            // if (oldOrder == null)
            // {
            //     response.Message = "Order not found";
            //     return response;
            // }
            //
            // order.Total = await GetTotalByProduct(dto);
            // order.Quantity = dto.Quantity;
            // order.CustomerNumber = dto.CustomerNumber;
            // order.ProductNumber = dto.ProductNumber;
            // order.ReturnStatus = dto.ReturnStatus;
            //
            var updatedOrder = await _orderRepository.UpdateAsync(order);
           
            response.Data = updatedOrder;
            response.Message = "Order updated successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Status = false;
            Console.WriteLine($"Failed to updated order with id: {order.Id} - {e}");
            return response;
        }
    }
    
    public async Task<ApiResponse<Order>>  DeleteOrderById(long id, long customerId)
    {
        var response = new ApiResponse<Order>();
        try
        {
            var order = await _orderRepository.GetByIdAsync(id,customerId);
            
            if (order == null)
            {
                response.Message = "Order not found";
                return response;
            }
           
            await _orderRepository.DeleteAsync(order);
           
            response.Data = null;
            response.Message = "Order deleted successfully";
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
}