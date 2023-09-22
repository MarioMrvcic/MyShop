using Microsoft.AspNetCore.Components;
using MyShop.Interfaces;
using Radzen;

namespace MyShop.Components.Category;

public partial class UpdateCategory
{
    [Inject]
    public ICategoryService CategoryService { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    [Inject]
    public NotificationService NotificationService { get; set; }


    public Entities.Category Model { get; set; } = new();
    [Parameter]
    public int Id { get; set; }
    public Entities.Category beforeUpdate { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Model = await CategoryService.GetCategoryByIdAsync(Id);

        beforeUpdate.Name = Model.Name;
        beforeUpdate.DisplayOrder = Model.DisplayOrder;

    }

    public async Task OnSubmit(Entities.Category model)
    {
        var addedCategory = await CategoryService.UpdateCategory(Id, Model);
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

    public void Cancel()
    {
        NavigationManager.NavigateTo("/categories");
    }

    public bool ValidateName(string name)
    {
        if (beforeUpdate.Name == Model.Name)
        {
            return true;
        }

        var check = Task.Run(async () => await CategoryService.CheckCategoryName(name));
        var checkResult = check.GetAwaiter().GetResult();

        return checkResult;
    }


    public bool ValidateDisplayOrder(int displayOrder)
    {
        if (beforeUpdate.DisplayOrder == Model.DisplayOrder)
        {
            return true;
        }

        var check = Task.Run(async () => await CategoryService.CheckCategoryDisplayOrder(displayOrder));
        var checkResult = check.GetAwaiter().GetResult();

        return checkResult;
    }
}
