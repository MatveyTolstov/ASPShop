using System.ComponentModel.DataAnnotations;

namespace ShopHunter.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Не указан Логин")]

    public string Login { get; set; }

    [Required(ErrorMessage = "Не указан Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}