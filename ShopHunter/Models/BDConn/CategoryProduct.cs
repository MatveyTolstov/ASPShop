using System.ComponentModel.DataAnnotations;
using ShopHunter.Models.BDConn;

namespace ShopHunter.BDConn;

public class CategoryProduct
{
    [Key] public int ID_CategoryProduct { get; set; }

    public string Category { get; set; }

    public Product Product { get; set; }
}