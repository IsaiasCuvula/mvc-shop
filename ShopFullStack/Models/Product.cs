using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShopFullStack.Models;

[Table("products")]
public class Product
{
    [Column("id")]
    public long Id { get; set; }
    [Column("name"), MaxLength(254)] 
    public required string Name { get; set; }
    [Column("brand"), MaxLength(254)]
    public required string Brand { get; set; }
    [Column("description"), MaxLength(508)]
    public required string Description { get; set; }
    [Column("price")]
    public decimal Price { get; set; }
    [Column("expiration_date")]
    public DateTime ExpirationDate { get; set; }

    [Column("stock_quantity")]
    public int Stock { get; set; }
    [Column("product_number")]
    public long ProductNumber { get; set; }
    
    
    [Column("imageUrl")]
    public string? ImageUrl { get; set; } 
    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    [ValidateNever, Column("order_id")] 
    public ICollection<Order> Orders { get; set; } = new List<Order>();

}