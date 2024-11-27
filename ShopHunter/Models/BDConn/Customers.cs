using System.ComponentModel.DataAnnotations;

namespace ShopHunter.Models.BDConn;

public class Customers
{
    [Key] public int ID_Customers { get; set; }

    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string MidName { get; set; }
    public int Users_ID { get; set; }

    public Users User { get; set; }
}