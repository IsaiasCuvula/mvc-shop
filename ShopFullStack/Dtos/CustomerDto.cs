namespace ShopApi.Dtos;

public record CustomerDto(
   string Name,
   long IdCardNumber,
   string City ,
   string Address ,
   string Phone ,
   string Email
);