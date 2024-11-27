using System.ComponentModel.DataAnnotations;

namespace ShopHunter.Models.BDConn;

public class Roles
{
    [Key] public int ID_Roles { get; set; }

    public string Role { get; set; }
}