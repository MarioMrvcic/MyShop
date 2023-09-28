using Microsoft.AspNetCore.Components;
using MyShop.Interfaces;
using Radzen;

namespace MyShop.Components.Category;

public partial class CategoryList : ComponentBase
{
    [Inject]
    public ICategoryService CategoryService { get; set; }
    [Inject]
    public IProductService ProductService { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    [Inject]
    public DialogService DialogService { get; set; }
    [Inject]
    public NotificationService NotificationService { get; set; }

    public List<Entities.Category>? Categories { get; set; }
    private IList<Entities.Category>? SelectedRow = null;
    private bool ShowAlert = true;
    private bool ShowAlertWhenDeleting = false;
    private bool ShowAlertDeletingCategoryWithProducts = false;

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

        Categories = await CategoryService.GetCategoriesAsync();
        ShowAlert = false;

        SelectedRow = Categories!.Take(1).ToList();
    }

    private void GoToCategoryDetails(int categoryId)
    {
        NavigationManager.NavigateTo($"categories/{categoryId}");
    }

    private void AddCategory()
    {
        NavigationManager.NavigateTo($"categories/AddCategory");
    }

    private void UpdateCategory(int categoryId)
    {
        NavigationManager.NavigateTo($"categories/UpdateCategory/{categoryId}");
    }

    private async void DeleteCategory(int categoryId)
    {

        var confirmDelete = await DialogService.Confirm($"Delete item with id {SelectedRow[0].Id}?", "Confirm", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (confirmDelete.GetValueOrDefault())
        {
            List<Entities.Product> ProductsWithCategory = await ProductService.GetProductByCategory(SelectedRow[0].Id);
            if (ProductsWithCategory.Count > 0)
            {
                string names = "";
                foreach (var product in ProductsWithCategory)
                {
                    names += product.Name + Environment.NewLine;
                }
                DialogService.Alert($"Category has products, delete them and try again!!!!", "Error!", new AlertOptions() { OkButtonText = "Ok" });
            }
            else
            {
                ShowAlertWhenDeleting = true;
                var DeletSucces = await CategoryService.DeleteCategory(categoryId);
                ShowAlertWhenDeleting = false;
                NavigationManager.NavigateTo($"categories", true);
            }

        }
    }
}
