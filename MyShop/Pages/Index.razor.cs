using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyShop.Entities;
using MyShop.Interfaces;
using Radzen.Blazor;
using Radzen;
using System.Security.Claims;
using MyShop.Services;
using MyShop.Components.Product;

namespace MyShop.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public ICartProductService CartProductService { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }
        [Inject]
        public NotificationService NotificationService { get; set; }

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

        private async void AddProductToCartProduct(int productId)
        {
            if (userIsLoggedIn)
            {
                var addedProductToCart = await CartProductService.AddProductToCartProduct(productId, userName);
                if (addedProductToCart is not null)
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Product added to cart", Detail = "", Duration = 4000 });
                }
            }
            else
            {
                var confirmAdd = await DialogService.Confirm($"Please log into your account to add products to cart", "You are not logged in", new ConfirmOptions() { OkButtonText = "Login", CancelButtonText = "Ok" });
                if (confirmAdd == true) {
                    NavigationManager.NavigateTo("identity/account/login", true);
                }
            }
        }

        void ShowNotification(NotificationMessage message)
        {
            NotificationService.Notify(message);
        }
    }
}
