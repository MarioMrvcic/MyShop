using System.ComponentModel.DataAnnotations;

namespace MyShop.Entities;

public class Category
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string? Name { get; set; }

    [Required]
    [Range(0, 10)]
    public int DisplayOrder { get; set; }
}
