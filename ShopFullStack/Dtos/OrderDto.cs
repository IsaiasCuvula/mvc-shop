using ShopFullStack.Utilities;

namespace ShopFullStack.Dtos;

public record OrderDto(
        long CustomerNumber,
        long ProductNumber,
        int Quantity,
        ReturnStatus ReturnStatus
);