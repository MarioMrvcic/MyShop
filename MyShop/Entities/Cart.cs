namespace MyShop.Entities;

public class Cart
{
    public int Id { get; set; }

    public CartStatus CartStatus { get; set; }

    public ShopAppWebUser? ShopAppWebUser { get; set; }

    public List<CartProduct>? Products { get; set; }
}
