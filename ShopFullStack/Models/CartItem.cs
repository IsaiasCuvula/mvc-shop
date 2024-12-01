using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShopFullStack.Models;

public class CartItem
{
    [Column("cart_item_id")]
    public long CartItemId { get; set; }
   
    [Column("quantity")]
    public int Quantity { get; set; }
    
    [Column("total")]
    public decimal Total { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    
    [ValidateNever,Column("product")]
    public Product Product { get; set; }
    
    [Column("cart_id")]
    public long CartId { get; set; }
    [ValidateNever, Column("cart")]
    public Cart Cart { get; set; }
    
}