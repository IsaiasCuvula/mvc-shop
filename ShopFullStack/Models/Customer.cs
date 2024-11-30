using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShopFullStack.Models;

[Table("customers")]
public class Customer
{   
    [Column("id")]
    public long  Id { get; set; } 
    
    [Column("name"),MaxLength(100)]
    public required string Name { get; set; } 
    [Column("id_card_number")]
    public long IdCardNumber { get; set; }
    [Column("city"),MaxLength(254)]
    public required string City { get; set; }
    [Column("address"),MaxLength(254)]
    public required string Address { get; set; } 
    [Column("phone"),MaxLength(20)]
    public required string Phone { get; set; } 
    [Column("email"),MaxLength(254)]
    public required string Email { get; set; } 
    [Column("customer_number")]
    public long CustomerNumber { get; set; }
 
    [Column("orders_id"), JsonIgnore]
    public ICollection<Order> Orders { get; set; }
    
    [Column("user_id")]
    public string AppUserId { get; set; }
    [ValidateNever, Column("app_user")] 
    public AppUser? AppUser { get; set; }
}