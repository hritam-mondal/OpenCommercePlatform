using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[PrimaryKey("RoleId", "PageId")]
[Table("page_roles", Schema = "navigation")]
public partial class PageRole
{
    [Key]
    [Column("role_id")]
    public int RoleId { get; set; }

    [Key]
    [Column("page_id")]
    public int PageId { get; set; }

    [Column("is_visible")]
    public bool IsVisible { get; set; }

    [Column("is_readable")]
    public bool IsReadable { get; set; }

    [Column("is_writable")]
    public bool IsWritable { get; set; }

    [ForeignKey("PageId")]
    [InverseProperty("PageRoles")]
    public virtual Page Page { get; set; } = null!;

    [ForeignKey("RoleId")]
    [InverseProperty("PageRoles")]
    public virtual Role Role { get; set; } = null!;
}
