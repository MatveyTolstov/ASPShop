using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopHunter.BDConn;

namespace ShopHunter.Models.BDConn;

public class Product
{
    [Key] public int ID_Product { get; set; }

    [Required] public string ProductName { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

    // Связь с категорией
    [ForeignKey("CategoryProduct")] public int CategoryProduct_ID { get; set; }

    public CategoryProduct CategoryProduct { get; set; }

    // Связь с изображением
    [ForeignKey("FK_Product_PictureProduct")]
    public int Picture_ID { get; set; }

    public PictureProduct PictureProduct { get; set; }
}