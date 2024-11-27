using System.ComponentModel.DataAnnotations;

namespace ShopHunter.Models.BDConn;

public class UserSite
{
    [Key] public int ID_Users { get; set; }
    [Required(ErrorMessage = "Не указан Login")]
    public string Login { get; set; }
    [Required(ErrorMessage = "Не указан Password")]
    public string Password { get; set; }
    [Required(ErrorMessage = "Не указан Roles_ID")]
    public int Roles_ID { get; set; }
    
}