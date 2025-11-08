using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[Table("discounts", Schema = "pricing")]
public partial class Discount
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("discount_per")]
    [Precision(10, 2)]
    public decimal? DiscountPer { get; set; }

    [Column("discount_amt")]
    [Precision(18, 2)]
    public decimal? DiscountAmt { get; set; }

    [Column("is_applicable")]
    public bool? IsApplicable { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("remarks")]
    [StringLength(500)]
    public string? Remarks { get; set; }

    [Column("store_id")]
    public int? StoreId { get; set; }

    [ForeignKey("StoreId")]
    [InverseProperty("Discounts")]
    public virtual Store? Store { get; set; }
}
