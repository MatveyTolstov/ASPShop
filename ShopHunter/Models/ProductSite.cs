using System.ComponentModel.DataAnnotations;

namespace ShopHunter.Models;

public class ProductSite
{
    [Key] public int ID_Product { get; set; }

    [Required(ErrorMessage = "Не указан Название")]
    public string ProductName { get; set; }

    [Required(ErrorMessage = "Не указан Количество")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Не указан Цена")]
    public int Price { get; set; }

    [Required(ErrorMessage = "Не указан CategoryProduct_ID")]
    public int CategoryProduct_ID { get; set; }

    [Required(ErrorMessage = "Не указан Picture_ID")]
    public int Picture_ID { get; set; }
}