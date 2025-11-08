using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[Table("stocks", Schema = "catalog")]
[Index("ItemId", Name = "idx_stocks_item")]
[Index("StoreId", Name = "idx_stocks_store")]
public partial class Stock
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("item_id")]
    public int? ItemId { get; set; }

    [Column("item_quantity")]
    [Precision(18, 3)]
    public decimal? ItemQuantity { get; set; }

    [Column("rate")]
    [Precision(18, 2)]
    public decimal Rate { get; set; }

    [Column("rate_for")]
    [StringLength(200)]
    public string? RateFor { get; set; }

    [Column("initial_unit_id")]
    public int? InitialUnitId { get; set; }

    [Column("store_id")]
    public int? StoreId { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("is_available")]
    public bool IsAvailable { get; set; }

    [Column("reason")]
    [StringLength(500)]
    public string? Reason { get; set; }

    [Column("sub_category_id")]
    public int? SubCategoryId { get; set; }

    [Column("discount_remarks")]
    [StringLength(500)]
    public string? DiscountRemarks { get; set; }

    [Column("discount_per")]
    [Precision(10, 2)]
    public decimal? DiscountPer { get; set; }

    [Column("is_discount_applicable")]
    public bool IsDiscountApplicable { get; set; }

    [ForeignKey("InitialUnitId")]
    [InverseProperty("Stocks")]
    public virtual Unit? InitialUnit { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("Stocks")]
    public virtual Item? Item { get; set; }

    [ForeignKey("StoreId")]
    [InverseProperty("Stocks")]
    public virtual Store? Store { get; set; }

    [ForeignKey("SubCategoryId")]
    [InverseProperty("Stocks")]
    public virtual SubCategory? SubCategory { get; set; }
}
