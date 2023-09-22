using MyShop.Entities;

namespace MyShop.Data;

public class DataBaseInitialization
{
   public static List<Category> ReturnCategories()
    {
        List<Category> _categories = new List<Category>(){
            new Category() { Id=1, Name = "Eau de Parfum", DisplayOrder=1},
            new Category() { Id=2, Name = "Eau de Toilettem", DisplayOrder=2},
            new Category() { Id=3, Name = "Eau de Cologne", DisplayOrder=3},
            new Category() { Id=4, Name = "Perfume", DisplayOrder=4}
        };

        return _categories;
    }
}
