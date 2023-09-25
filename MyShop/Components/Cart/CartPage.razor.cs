using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyShop.Entities;
using MyShop.Services;
using System.Security.Claims;

namespace MyShop.Components.Cart;

public partial class CartPage : ComponentBase
{
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    //ime current usera
    string userName;

    //provjera je li user uopce ulogiran
    private bool userIsLoggedIn;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authenticationState.User;

        userIsLoggedIn = user.Identity.IsAuthenticated;

        if (userIsLoggedIn)
        {
            userName = user.Identity.Name;
        }

    }

}
