using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Entities;

public class CartProduct
{
    [Key]
    public int CartId { get; set; }
    public int ProductsId { get; set; }
    public Cart Cart { get; set; } = null!;
    public Product Product { get; set; } = null!;

    public int ProductQuantity { get; set; }
}
