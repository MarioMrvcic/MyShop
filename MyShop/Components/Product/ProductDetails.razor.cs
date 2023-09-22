using Microsoft.AspNetCore.Components;
using MyShop.Interfaces;

namespace MyShop.Components.Product;

public partial class ProductDetails : ComponentBase
{
    [Inject]
    public IProductService ProductService { get; set; }


    [Parameter]
    public int Id { get; set; }

    public Entities.Product? Product { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Product = await ProductService.GetProductByIdAsync(Id);
    }
}
