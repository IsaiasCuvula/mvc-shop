namespace ShopFullStack.Dtos;

public record ProductDto(
         string Name,
         string Brand,
         string Description,
         decimal Price,
         DateTime ExpirationDate,
         int Quantity,
         string Image 
    );