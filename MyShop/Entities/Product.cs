using System.ComponentModel.DataAnnotations;

namespace MyShop.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string? Name { get; set; }

    [MaxLength(100)]
    public string? BrandName { get; set; }

    public decimal? Price { get; set; }

    public int? QuantityInStock { get; set; }

    public string? AssociatedImage { get; set; }

    public int? CategoryId { get; set; }

    public Category? Category { get; set; }
}
