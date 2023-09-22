using Microsoft.AspNetCore.Components;
using MyShop.Interfaces;
using Radzen;
using System.Globalization;

namespace MyShop.Components.Product;

public partial class UpdateProduct : ComponentBase
{
    [Inject]
    public ICategoryService CategoryService { get; set; }
    [Inject]
    public IProductService ProductService { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    [Inject]
    public NotificationService NotificationService { get; set; }


    //valuta
    CultureInfo cultureInfo = new("fr-FR");

    public List<Entities.Category> Categories { get; set; }
    public Entities.Product Model { get; set; } = new();
    bool popup;
    [Parameter]
    public int Id { get; set; }
    public Entities.Product beforeUpdate { get; set; } = new();

    //input
    string fileName;
    long? fileSize;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Categories = await CategoryService.GetCategoriesAsync();
        Model = await ProductService.GetProductByIdAsync(Id);

        beforeUpdate.Name = Model.Name;
        beforeUpdate.BrandName = Model.BrandName;
        beforeUpdate.Price = Model.Price;
        beforeUpdate.QuantityInStock = Model.QuantityInStock;
        beforeUpdate.CategoryId = Model.CategoryId;
        beforeUpdate.AssociatedImage = Model.AssociatedImage;

    }

    public async Task OnSubmit(Entities.Product model)
    {
        var addedProduct = await ProductService.UpdateProduct(Id, Model);
        if (addedProduct is not null)
        {
            NavigationManager.NavigateTo("/products");
            ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success Summary", Detail = "Success Detail", Duration = 4000 });
        }
    }

    void OnInvalidSubmit(FormInvalidSubmitEventArgs args)
    {
    }

    void ShowNotification(NotificationMessage message)
    {
        NotificationService.Notify(message);
    }

    public void Cancle()
    {
        NavigationManager.NavigateTo("/products");
    }
}
