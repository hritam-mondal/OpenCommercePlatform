using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCP.API.Models;

[Table("addresses", Schema = "orders")]
public partial class Address
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_id")]
    [StringLength(50)]
    public string? OrderId { get; set; }

    [Column("user_id")]
    public long? UserId { get; set; }

    [Column("full_name")]
    [StringLength(200)]
    public string? FullName { get; set; }

    [Column("address")]
    public string? UserAddress { get; set; }

    [Column("city")]
    [StringLength(200)]
    public string? City { get; set; }

    [Column("pin")]
    [StringLength(50)]
    public string? Pin { get; set; }

    [Column("phone")]
    [StringLength(50)]
    public string? Phone { get; set; }

    [Column("email")]
    [StringLength(200)]
    public string? Email { get; set; }

    [Column("address_type")]
    [StringLength(50)]
    public string? AddressType { get; set; }

    [Column("state")]
    [StringLength(200)]
    public string? State { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Addresses")]
    public virtual Order? Order { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Addresses")]
    public virtual User? User { get; set; }
}
