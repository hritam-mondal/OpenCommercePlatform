using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[Table("orders", Schema = "orders")]
[Index("StoreId", Name = "idx_orders_store")]
[Index("UserId", Name = "idx_orders_user")]
public partial class Order
{
    [Key]
    [Column("order_id")]
    [StringLength(50)]
    public string OrderId { get; set; } = null!;

    [Column("order_name")]
    [StringLength(200)]
    public string? OrderName { get; set; }

    [Column("order_date")]
    public DateTime? OrderDate { get; set; }

    [Column("total_price")]
    [Precision(18, 2)]
    public decimal? TotalPrice { get; set; }

    [Column("user_id")]
    public long UserId { get; set; }

    [Column("order_note")]
    [StringLength(500)]
    public string? OrderNote { get; set; }

    [Column("order_status")]
    [StringLength(50)]
    public string? OrderStatus { get; set; }

    [Column("payment_status")]
    [StringLength(50)]
    public string? PaymentStatus { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("bank_tran_id")]
    [StringLength(200)]
    public string? BankTranId { get; set; }

    [Column("remarks")]
    public string? Remarks { get; set; }

    [Column("payment_mode")]
    [StringLength(50)]
    public string? PaymentMode { get; set; }

    [Column("gateway_tran_id")]
    [StringLength(200)]
    public string? GatewayTranId { get; set; }

    [Column("bank_name")]
    [StringLength(200)]
    public string? BankName { get; set; }

    [Column("tran_status")]
    [StringLength(200)]
    public string? TranStatus { get; set; }

    [Column("store_id")]
    public int? StoreId { get; set; }

    [Column("final_price")]
    [Precision(18, 2)]
    public decimal? FinalPrice { get; set; }

    [Column("overall_discount_per")]
    [Precision(10, 2)]
    public decimal? OverallDiscountPer { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderNotification> OrderNotifications { get; set; } = new List<OrderNotification>();

    [ForeignKey("StoreId")]
    [InverseProperty("Orders")]
    public virtual Store? Store { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User User { get; set; } = null!;
}
