using Microsoft.EntityFrameworkCore;
using MyShop.Entities;

namespace MyShop.Data;

public class AplicationDbContext : DbContext
{

    public AplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}
