using ShopApi.Dtos;
using ShopApi.Models;
using ShopApi.Utilities;

namespace ShopApi.Mappers;

public class OrderMapper
{
    public static Order MapToEntity(OrderDto dto)
    {
        return new Order
        {
            CustomerNumber = dto.CustomerNumber,
            ProductNumber = dto.ProductNumber,
            OrderDate = DateTime.Now.ToUniversalTime(),
            Quantity = dto.Quantity,
            PaymentStatus = PaymentStatus.Unpaid,
            ReturnStatus = dto.ReturnStatus
        };
    }
}