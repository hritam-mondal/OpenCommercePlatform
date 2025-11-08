using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[PrimaryKey("Pin", "StoreId")]
[Table("pincodes", Schema = "geo")]
public partial class Pincode
{
    [Key]
    [Column("pin")]
    [StringLength(50)]
    public string Pin { get; set; } = null!;

    [Column("deliver_day")]
    [StringLength(50)]
    public string? DeliverDay { get; set; }

    [Key]
    [Column("store_id")]
    public int StoreId { get; set; }

    [ForeignKey("StoreId")]
    [InverseProperty("Pincodes")]
    public virtual Store Store { get; set; } = null!;
}
