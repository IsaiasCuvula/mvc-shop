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
    
    [Column("name")]
    [Required(ErrorMessage = "The name field is required.")]
    [MaxLength(100, ErrorMessage = "The name cannot exceed 100 characters.")]
    public required string Name { get; set; }

    [Column("id_card_number")]
    [Required(ErrorMessage = "The ID card number is required.")]
    public long IdCardNumber { get; set; }

    [Column("city")]
    [Required(ErrorMessage = "The city field is required.")]
    [MaxLength(254, ErrorMessage = "The city cannot exceed 254 characters.")]
    public required string City { get; set; }

    [Column("address")]
    [Required(ErrorMessage = "The address field is required.")]
    [MaxLength(254, ErrorMessage = "The address cannot exceed 254 characters.")]
    public required string Address { get; set; }

    [Column("phone")]
    [Required(ErrorMessage = "The phone field is required.")]
    [MaxLength(20, ErrorMessage = "The phone number cannot exceed 20 characters.")]
    public required string Phone { get; set; }

    [Column("email")]
    [Required(ErrorMessage = "The email field is required.")]
    [MaxLength(254, ErrorMessage = "The email cannot exceed 254 characters.")]
    [EmailAddress(ErrorMessage = "The email format is invalid.")]
    public required string Email { get; set; }

    [Column("customer_number")]
    public long CustomerNumber { get; set; }
 
    [ValidateNever, Column("orders_id"), JsonIgnore]
    public ICollection<Order> Orders { get; set; }
    
    [Column("user_id")]
    public string AppUserId { get; set; }
    
    [ValidateNever, Column("carts"), JsonIgnore]
    public Cart Cart { get; set; }
}