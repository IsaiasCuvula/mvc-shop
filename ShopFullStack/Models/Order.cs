using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ShopFullStack.Utilities;

namespace ShopFullStack.Models;

[Table("orders")]
public class Order
{
   [Column("id")]
   public long Id { get; set; }

   [Column("customer_id")]
   public long CustomerId { get; set; }
        
   [ValidateNever, Column("customer")]
   public Customer? Customer { get; set; }
        
   [Column("cart_id")]
   public long CartId { get; set; }

   [ValidateNever, Column("cart")]
   public Cart? Cart { get; set; }

   [Column("order_items_id")]
   public ICollection<CartItem> OrderItems { get; set; } = new List<CartItem>();

   [Column("total")]
   public decimal Total { get; set; }
        
   [Column("status")]
   public OrderStatus Status { get; set; } = OrderStatus.Pending;
   
   [Column("payment_status")]
   public PaymentStatus OrderPaymentStatus { get; set; } = PaymentStatus.Unpaid; 

   [Column("order_returned_status")]
   public ReturnStatus OrderReturnedStatus { get; set; } = ReturnStatus.NotReturned; 

   [Column("shipping_address")]
   public string ShippingAddress { get; set; } = string.Empty;

   [Column("created_at")]
   public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
        
   [Column("shipped_at")]
   public DateTime? ShippedAt { get; set; }
}