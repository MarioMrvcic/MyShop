using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Entities;

public class ShopAppWebUser : IdentityUser
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    public string? Address { get; set; } = "";
    [Required]
    public string? City { get; set; }
    [Required]
    public int? PostalCode { get; set; }
}
