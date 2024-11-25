namespace ShopApi.Dtos;

public record ProductDto(
         string Brand,
         string Description,
         decimal Price,
         DateTime ExpirationDate,
         int Quantity,
         string Image 
    );