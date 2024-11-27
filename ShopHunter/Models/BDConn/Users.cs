using System.ComponentModel.DataAnnotations;

namespace ShopHunter.Models.BDConn;

public class Users
{
    [Key] public int ID_Users { get; set; }

    public string Login { get; set; }
    public string Password { get; set; }
    public int Roles_ID { get; set; }

    public Customers Customer { get; set; }
}