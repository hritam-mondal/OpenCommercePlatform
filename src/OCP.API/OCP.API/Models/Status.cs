using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCP.API.Models;

[PrimaryKey("StatusId", "StatusType")]
[Table("status", Schema = "meta")]
public partial class Status
{
    [Key]
    [Column("status_id")]
    [StringLength(50)]
    public string StatusId { get; set; } = null!;

    [Key]
    [Column("status_type")]
    [StringLength(50)]
    public string StatusType { get; set; } = null!;

    [Column("status_text")]
    [StringLength(200)]
    public string? StatusText { get; set; }

    [Column("description")]
    public string? Description { get; set; }
}
