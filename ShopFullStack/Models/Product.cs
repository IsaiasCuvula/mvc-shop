using System.ComponentModel.DataAnnotations.Schema;
namespace ShopApi.Models;

[Table("products")]
public class Product
{
    [Column("id")]
    public long Id { get; set; }
    [Column("brand")]
    public required string Brand { get; set; }
    [Column("description")]
    public required string Description { get; set; }
    [Column("price")]
    public decimal Price { get; set; }
    [Column("expiration_date")]
    public DateTime ExpirationDate { get; set; }
    //This is the quantity available in stock
    [Column("quantity")]
    public int Quantity { get; set; }
    [Column("product_number")]
    public long ProductNumber { get; set; }
    [Column("image")]
    public required string Image { get; set; }
}