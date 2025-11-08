using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[Table("user_roles", Schema = "security")]
[Index("RoleId", Name = "idx_user_roles_role")]
[Index("UserId", Name = "idx_user_roles_user")]
public partial class UserRole
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public long UserId { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("UserRoles")]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserRoles")]
    public virtual User User { get; set; } = null!;
}
