using Microsoft.AspNetCore.Components;
using MyShop.Interfaces;
using Radzen;
using System.Globalization;

namespace MyShop.Components.Product;

public partial class AddProduct : ComponentBase
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
    CultureInfo hr = new CultureInfo("fr-FR");

    public Entities.Product Model { get; set; } = new();
    public List<Entities.Category> Categories { get; set; } = new();
    public int CategoryId;
    bool popup;

    //input
    string fileName;
    long? fileSize;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Categories = await CategoryService.GetCategoriesAsync();


    }

    public async Task OnSubmit(Entities.Product model)
    {
        Model.CategoryId = CategoryId;
        var addedProduct = await ProductService.AddProduct(Model);
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
