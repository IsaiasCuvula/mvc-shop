using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ShopApi.Utilities;

namespace ShopApi.Models;

[Table("orders")]
public class Order
{
   [Column("id")]
   public long Id { get; set; } 
   [Column("customer_number")]
   public long CustomerNumber {get;set;}
   [Column("product_number")]
   public long ProductNumber { get; set; }
   [Column("quantity")]
   public int Quantity {get;set;}
   [Column("order_date")]
   public DateTime OrderDate {get;set;}
   [Column("payment_date")]
   public DateTime PaymentDate {get;set;}
   [Column("payment_status")]
   public PaymentStatus PaymentStatus {get;set;}
   [Column("return_status")]
   public ReturnStatus ReturnStatus {get;set;}
   [Column("total")]
   public decimal Total {get;set;}
   [Column("group_order_id")]
   public String GroupOrderId { get; set; }
   
   [Column("customer_id"), JsonIgnore]
   public Customer Customer {get;set;}
}