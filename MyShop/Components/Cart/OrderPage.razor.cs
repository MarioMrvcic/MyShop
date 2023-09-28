using Microsoft.AspNetCore.Components;
using MyShop.Entities;
using MyShop.Interfaces;
using MyShop.Services;
using Radzen;
using System.Globalization;

namespace MyShop.Components.Cart;

public partial class OrderPage : ComponentBase
{
    [Inject]
    public ICartProductService CartProductService { get; set; }

    [Inject]
    public IProductService ProductService { get; set; }


    //used to store users products
    public List<Entities.CartProduct> CartProducts { get; set; } = new();

    //valuta
    public CultureInfo cultureInfo = new("fr-FR");

    //parameter passed trough URL
    [Parameter]
    public int cartId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        CartProducts = await CartProductService.GetAllCartProductEntriesForCartByCartIdAsync(cartId);

        foreach (CartProduct cp in CartProducts)
        {
            cp.Product = await ProductService.GetProductByIdAsync(cp.ProductsId);
        }
    }
}