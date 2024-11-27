using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopHunter.Models.BDConn;

[Table("PictureProduct")]
public class PictureProduct
{
    [Key] public int ID_PictureProduct { get; set; }

    public byte[] Cover { get; set; }

    [NotMapped] public string Base64Cover { get; set; }

    public string CoverFormat { get; set; }
    public Product Product { get; set; }
}