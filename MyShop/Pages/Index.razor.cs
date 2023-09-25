using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyShop.Entities;
using MyShop.Interfaces;
using Radzen.Blazor;
using Radzen;
using System.Security.Claims;
using MyShop.Services;

namespace MyShop.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public ICartService CartService { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }

        public List<Product>? Products { get; set; } = new();

        //info about user
        bool userIsLoggedIn;
        string userName;
        ClaimsPrincipal user;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Products = await ProductService.GetProductsAsync();

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            user = authenticationState.User;
            userIsLoggedIn = user.Identity.IsAuthenticated;
            if (userIsLoggedIn)
            {
                userName = user.Identity.Name;
            }
        }

        private void GoToProductDetails(int productId)
        {
            NavigationManager.NavigateTo($"products/{productId}");
        }

        private void UpdateProduct(int productId)
        {
            NavigationManager.NavigateTo($"products/UpdateProduct/{productId}");
        }

        private async void AddProductToCart(int productId)
        {
            if (userIsLoggedIn)
            {
                CartService.AddProductToCart(productId, userName);
            }
            else
            {
                var confirmAdd = await DialogService.Confirm($"Please login to add products to cart", "Confirm", new ConfirmOptions() { OkButtonText = "Login", CancelButtonText = "Home" });
                if (confirmAdd == true) {
                    NavigationManager.NavigateTo("identity/account/login", true);
                }
            }
        }
    }
}
