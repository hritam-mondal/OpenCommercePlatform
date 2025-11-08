using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[PrimaryKey("OrderId", "UserId")]
[Table("order_notifications", Schema = "orders")]
public partial class OrderNotification
{
    [Key]
    [Column("order_id")]
    [StringLength(50)]
    public string OrderId { get; set; } = null!;

    [Key]
    [Column("user_id")]
    public long UserId { get; set; }

    [Column("store_id")]
    public int? StoreId { get; set; }

    [Column("is_notify")]
    public bool? IsNotify { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderNotifications")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("StoreId")]
    [InverseProperty("OrderNotifications")]
    public virtual Store? Store { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("OrderNotifications")]
    public virtual User User { get; set; } = null!;
}
