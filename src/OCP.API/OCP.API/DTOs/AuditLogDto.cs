namespace OCP.API.DTOs;

public sealed class AuditLogDto
{
    public int Id { get; set; }
    public string? RefId { get; set; }
    public string? UserIdentifier { get; set; }
    public string? Activity { get; set; }
    public DateTimeOffset ActivityDate { get; set; }
}
