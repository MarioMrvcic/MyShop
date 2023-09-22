using Microsoft.AspNetCore.Components;
using MyShop.Interfaces;
using Radzen;

namespace MyShop.Components.Category;

public partial class AddCategory : ComponentBase
{
    [Inject]
    public ICategoryService CategoryService { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    [Inject]
    public NotificationService NotificationService { get; set; }

    public Entities.Category Model { get; set; } = new();

    public async Task OnSubmit(Entities.Category model)
    {
        var addedCategory = await CategoryService.AddCategory(Model);
        if (addedCategory is not null)
        {
            NavigationManager.NavigateTo("/categories");
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
        NavigationManager.NavigateTo("/categories");
    }

    public bool ValidateName(string name)
    {
        var check = Task.Run(async () => await CategoryService.CheckCategoryName(name));
        var checkResult = check.GetAwaiter().GetResult();

        return checkResult;
    }


    public bool ValidateDisplayOrder(int id)
    {
        var check = Task.Run(async () => await CategoryService.CheckCategoryDisplayOrder(id));
        var checkResult = check.GetAwaiter().GetResult();

        return checkResult;
    }
}
