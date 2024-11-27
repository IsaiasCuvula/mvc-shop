namespace ShopFullStack.Dtos;

public record UserOrdersDto(
    decimal DeliveryPrice,
    List<OrderDto> Orders
);
