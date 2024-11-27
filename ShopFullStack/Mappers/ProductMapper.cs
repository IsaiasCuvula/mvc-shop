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
            Name = dto.Name,
            Brand = dto.Brand,
            ImageUrl = dto.Image,
            ProductNumber = AppHelpers.GenerateRandomNumber(),
            Stock = dto.Quantity,
            ExpirationDate = dto.ExpirationDate.ToUniversalTime(),
            Price = dto.Price,
            Description = dto.Description 
        };
    }
}