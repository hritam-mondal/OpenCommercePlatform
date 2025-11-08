using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[PrimaryKey("PhoneNo", "Otp")]
[Table("phones", Schema = "comm")]
[Index("ApplicationUserId", Name = "idx_phones_user")]
public partial class Phone
{
    [Key]
    [Column("phone_no")]
    [StringLength(50)]
    public string PhoneNo { get; set; } = null!;

    [Key]
    [Column("otp")]
    [StringLength(50)]
    public string Otp { get; set; } = null!;

    [Column("expire_date")]
    public DateTime? ExpireDate { get; set; }

    [Column("is_verified")]
    public bool? IsVerified { get; set; }

    [Column("application_user_id")]
    public long? ApplicationUserId { get; set; }

    [ForeignKey("ApplicationUserId")]
    [InverseProperty("Phones")]
    public virtual User? ApplicationUser { get; set; }
}
