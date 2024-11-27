using ShopHunter.Models.BDConn;

namespace ShopHunter.Models;

public class Cart
{
    public Cart()
    {
        CarLines = new List<Product>();
    }

    public List<Product> CarLines { get; set; }

    public decimal TotalPrice
    {
        get
        {
            decimal totalPrice = 0;
            foreach (Product product in CarLines)
            {
                totalPrice += product.Price;
            }
            return totalPrice;
        }
    }
}