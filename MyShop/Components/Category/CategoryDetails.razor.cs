using Microsoft.AspNetCore.Components;
using MyShop.Interfaces;

namespace MyShop.Components.Category;

public partial class CategoryDetails
{
    [Inject]
    public ICategoryService CategoryService { get; set; }

    [Parameter]
    public int Id { get; set; }

    public Entities.Category? Category { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Category = await CategoryService.GetCategoryByIdAsync(Id);
    }
}
