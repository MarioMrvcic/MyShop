using Microsoft.AspNetCore.Components;
using MyShop.Entities;
using MyShop.Interfaces;
using MyShop.Services;
using Radzen;
using System.Globalization;

namespace MyShop.Components.Product;

public partial class ProductList : ComponentBase
{
    [Inject]
    public IProductService ProductService { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    [Inject]
    public DialogService DialogService { get; set; }
    [Inject]
    public NotificationService NotificationService { get; set; }


    //valuta
    CultureInfo cultureInfo = new("fr-FR");

    public List<Entities.Product>? Products { get; set; }
    private IList<Entities.Product>? SelectedRow = null;
    private bool ShowAlert = true;
    private bool ShowAlertWhenDeleting = false;

    //alert
    Variant variant = Variant.Filled;
    AlertSize size = AlertSize.Medium;
    AlertStyle alertStyle = AlertStyle.Primary;

    void ClearSelection()
    {
        SelectedRow = null;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Products = await ProductService.GetProductsAsync();
        ShowAlert = false;

        SelectedRow = Products!.Take(1).ToList();
    }

    private void GoToProductDetails(int productId)
    {
        NavigationManager.NavigateTo($"products/{productId}");
    }

    private void AddProduct()
    {
        NavigationManager.NavigateTo($"products/AddProduct");
    }

    private void UpdateProduct(int productId)
    {
        NavigationManager.NavigateTo($"products/UpdateProduct/{productId}");
    }

    private async void DeleteProduct(int productId)
    {
        var confirmDelete = await DialogService.Confirm($"Delete item with id {SelectedRow[0].Id}?", "Confirm", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (confirmDelete.GetValueOrDefault())
        {
            ShowAlertWhenDeleting = true;
            var DeletSucces = await ProductService.DeleteProduct(productId);
            ShowAlertWhenDeleting = false;
            NavigationManager.NavigateTo($"products", true);
        }
    }
}
