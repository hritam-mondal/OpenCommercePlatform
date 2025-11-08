using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[Table("store_users", Schema = "store")]
[Index("StoreId", Name = "idx_store_users_store")]
[Index("UserId", Name = "idx_store_users_user")]
public partial class StoreUser
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public long UserId { get; set; }

    [Column("store_id")]
    public int StoreId { get; set; }

    [ForeignKey("StoreId")]
    [InverseProperty("StoreUsers")]
    public virtual Store Store { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("StoreUsers")]
    public virtual User User { get; set; } = null!;
}
