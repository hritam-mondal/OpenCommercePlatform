using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCP.API.Models;

[Table("units", Schema = "catalog")]
public partial class Unit
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("unit_name")]
    [StringLength(100)]
    public string? UnitName { get; set; }

    [Column("unit_order")]
    public int? UnitOrder { get; set; }

    [Column("conversion_ratio")]
    public decimal? ConversionRatio { get; set; }

    [InverseProperty("Unit")]
    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    [InverseProperty("Unit")]
    public virtual ICollection<ItemUnit> ItemUnits { get; set; } = new List<ItemUnit>();

    [InverseProperty("Unit")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("InitialUnit")]
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
