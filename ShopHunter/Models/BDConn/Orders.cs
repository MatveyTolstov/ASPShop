using System.ComponentModel.DataAnnotations;

namespace ShopHunter.Models.BDConn;

public class Orders
{
    [Key] public int ID_Orders { get; set; }

    public int TotalPrice { get; set; }
    public DateTime Date { get; set; }
    public int Product_ID { get; set; }
    public int Customers_ID { get; set; }
}