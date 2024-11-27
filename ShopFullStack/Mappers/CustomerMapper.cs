using ShopFullStack.Dtos;
using ShopFullStack.Models;
using ShopFullStack.Utilities;

namespace ShopFullStack.Mappers;

public class CustomerMapper
{
    public static Customer MapToEntity(CustomerDto dto)
    {
        return new Customer
        {
            Name = dto.Name,
            IdCardNumber = dto.IdCardNumber,
            City = dto.City,
            Address = dto.Address,
            Phone = dto.Phone,
            Email = dto.Email,
            CustomerNumber = AppHelpers.GenerateRandomNumber()
        };
    }
}