using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCP.API.Models;

[Table("audit_logs", Schema = "audit")]
[Index("RefId", Name = "idx_audit_logs_ref_id")]
[Index("UserIdentifier", Name = "idx_audit_logs_user_identifier")]
public partial class AuditLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ref_id")]
    [StringLength(100)]
    public string? RefId { get; set; }

    [Column("user_identifier")]
    [StringLength(50)]
    public string? UserIdentifier { get; set; }

    [Column("activity")]
    public string? Activity { get; set; }

    [Column("activity_date")]
    public DateTime ActivityDate { get; set; }
}
