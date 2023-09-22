using Microsoft.AspNetCore.Components;
using MyShop.Entities;
using MyShop.Interfaces;

namespace MyShop.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }


        public List<Product>? Products { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Products = await ProductService.GetProductsAsync();

        }

        private void GoToProductDetails(int productId)
        {
            NavigationManager.NavigateTo($"products/{productId}");
        }

        private void UpdateProduct(int productId)
        {
            NavigationManager.NavigateTo($"products/UpdateProduct/{productId}");
        }
    }
}
