using ShopFullStack.Dtos;
using ShopFullStack.Models;
using ShopFullStack.Utilities;

namespace ShopFullStack.Mappers;

public class ProductMapper
{
    public static Product MapToEntity(ProductDto dto)
    {
        return new Product
        {
            Brand = dto.Brand,
            Image = dto.Image,
            ProductNumber = AppHelpers.GenerateRandomNumber(),
            Quantity = dto.Quantity,
            ExpirationDate = dto.ExpirationDate.ToUniversalTime(),
            Price = dto.Price,
            Description = dto.Description 
        };
    }
}