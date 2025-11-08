using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[Table("order_details", Schema = "orders")]
[Index("OrderId", Name = "idx_order_details_order")]
public partial class OrderDetail
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_id")]
    [StringLength(50)]
    public string OrderId { get; set; } = null!;

    [Column("item_id")]
    public int? ItemId { get; set; }

    [Column("item_name")]
    [StringLength(300)]
    public string? ItemName { get; set; }

    [Column("item_quantity")]
    [Precision(18, 3)]
    public decimal? ItemQuantity { get; set; }

    [Column("unit_id")]
    public int? UnitId { get; set; }

    [Column("rate")]
    [Precision(18, 2)]
    public decimal? Rate { get; set; }

    [Column("item_price")]
    [Precision(18, 2)]
    public decimal? ItemPrice { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("store_id")]
    public int? StoreId { get; set; }

    [Column("discount_per")]
    [Precision(10, 2)]
    public decimal? DiscountPer { get; set; }

    [Column("actual_rate")]
    [Precision(18, 2)]
    public decimal? ActualRate { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("OrderDetails")]
    public virtual Item? Item { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("StoreId")]
    [InverseProperty("OrderDetails")]
    public virtual Store? Store { get; set; }

    [ForeignKey("UnitId")]
    [InverseProperty("OrderDetails")]
    public virtual Unit? Unit { get; set; }
}
