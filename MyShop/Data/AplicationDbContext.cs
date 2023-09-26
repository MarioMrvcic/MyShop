using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyShop.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Data;

public class AplicationDbContext : IdentityDbContext<ShopAppWebUser>
{

    public AplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Cart> Carts { get; set; }

    public DbSet<CartProduct> CartProduct { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Category>().HasData(DataBaseInitialization.ReturnCategories());

        modelBuilder.Entity<CartProduct>().HasKey(cp => new { cp.CartId, cp.ProductsId});

        modelBuilder.Entity<Cart>().HasMany(c => c.Products).WithOne(c => c.Cart).HasForeignKey(c => c.CartId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>().HasMany(c => c.Carts).WithOne(c => c.Product).HasForeignKey(c => c.ProductsId).OnDelete(DeleteBehavior.Cascade);


    }
}
