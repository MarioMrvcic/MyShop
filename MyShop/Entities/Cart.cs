namespace MyShop.Entities;

public class Cart
{
    public int Id { get; set; }

    public CartStatus CartStatus { get; set; }

    public DateTime? FinishedCartOnDate { get; set; }

    public decimal CartCost { get; set; }

    public string ShopAppWebUserId { get; set; } 

    public ShopAppWebUser? ShopAppWebUser { get; set; }

    public List<CartProduct>? Products { get; set; }
}
