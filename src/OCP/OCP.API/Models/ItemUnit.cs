using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[PrimaryKey("ItemId", "UnitId", "StoreId")]
[Table("item_units", Schema = "catalog")]
public partial class ItemUnit
{
    [Key]
    [Column("item_id")]
    public int ItemId { get; set; }

    [Key]
    [Column("unit_id")]
    public int UnitId { get; set; }

    [Key]
    [Column("store_id")]
    public int StoreId { get; set; }

    [Column("is_exists_in_order")]
    public bool IsExistsInOrder { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ItemUnits")]
    public virtual Item Item { get; set; } = null!;

    [ForeignKey("StoreId")]
    [InverseProperty("ItemUnits")]
    public virtual Store Store { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("ItemUnits")]
    public virtual Unit Unit { get; set; } = null!;
}
