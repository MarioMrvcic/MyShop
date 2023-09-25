using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Interfaces;
using MyShop.Services;
using Radzen;
using Microsoft.AspNetCore.Identity;
using MyShop.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultIdentity<ShopAppWebUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<AplicationDbContext>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddHubOptions(o =>
{
    o.MaximumReceiveMessageSize = 1024 * 1024 * 10;
});
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddDbContext<AplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyShopConnectionString"));
});
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


//za deletat cart i usera
//DELETE FROM[MyShop].[dbo].[Carts]
//WHERE[ShopAppWebUserId] = 'd6a86494-66f5-4fc4-af68-0c2231de85f7';

//DELETE FROM[MyShop].[dbo].[AspNetUsers]
//WHERE[Id] = 'd6a86494-66f5-4fc4-af68-0c2231de85f7';