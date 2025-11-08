using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCP.API.Models;

[Table("cart_details", Schema = "cart")]
[Index("CartId", Name = "idx_cart_details_cart")]
public partial class CartDetail
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cart_id")]
    public int CartId { get; set; }

    [Column("item_id")]
    public int? ItemId { get; set; }

    [Column("item_quantity")]
    public int? ItemQuantity { get; set; }

    [Column("rate")]
    [Precision(18, 2)]
    public decimal? Rate { get; set; }

    [Column("item_price")]
    [Precision(18, 2)]
    public decimal? ItemPrice { get; set; }

    [Column("unit_id")]
    public int? UnitId { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartDetails")]
    public virtual Cart Cart { get; set; } = null!;

    [ForeignKey("ItemId")]
    [InverseProperty("CartDetails")]
    public virtual Item? Item { get; set; }

    [ForeignKey("UnitId")]
    [InverseProperty("CartDetails")]
    public virtual Unit? Unit { get; set; }
}
