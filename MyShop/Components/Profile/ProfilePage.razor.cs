using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyShop.Entities;
using MyShop.Interfaces;
using MyShop.Services;

namespace MyShop.Components.Profile;

public partial class ProfilePage : ComponentBase
{
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject]
    public IUserService UserService { get; set; }
    [Inject]
    public ICartService CartService { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    //checks if user is logged in
    private bool userIsLoggedIn;

    //current user name
    string userName;

    //whole user
    ShopAppWebUser user = new();

    List<Entities.Cart> pastUserCarts = new();

    protected override async Task OnInitializedAsync()
    {

        await base.OnInitializedAsync();
        var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var userClaimsPrincipal = authenticationState.User;

        userIsLoggedIn = userClaimsPrincipal.Identity.IsAuthenticated;

        if (userIsLoggedIn)
        {
            userName = userClaimsPrincipal.Identity.Name;
            user = await UserService.GetUserByNameAsync(userName);
        }

        pastUserCarts = await CartService.GetAllUserPastCarts(user.Id);
    }

    public void GetCartDetails(int cartId)
    {
        NavigationManager.NavigateTo($"Order/{cartId}");
    }

    public void UpdateProfile()
    {
        NavigationManager.NavigateTo("/Profile/UpdateProfile", true);
    }
}