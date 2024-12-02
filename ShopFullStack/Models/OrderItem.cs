using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShopFullStack.Models;

public class OrderItem
{
    [Column("order_item_id")]
    public long OrderItemId { get; set; }
    
    [Column("order_id")]
    public long OrderId { get; set; }
    
    [ValidateNever, Column("order")]
    public Order? Order { get; set; }

    [Column("product_id")]
    public long ProductId { get; set; }
    
    [ValidateNever, Column("product")]
    public Product? Product { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("price_per_item")]
    public decimal PricePerItem { get; set; }
    
    [Column("total")]
    public decimal Total { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();

}