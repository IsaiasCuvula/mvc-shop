using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShopFullStack.Models;

public class Cart
{
    [Column("id")]
    public long Id { get; set; }

    [Column("customer_id")]
    public long CustomerId { get; set; }
    
    [ValidateNever, Column("customer")]
    public Customer? Customer { get; set; }
    
    [Column("cart_items_id")]
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    
    [ValidateNever,Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
}