namespace OCP.API.DTOs;

public sealed class StatusDto
{
    public string StatusId { get; set; } = string.Empty;
    public string StatusType { get; set; } = string.Empty;
    public string? StatusText { get; set; }
    public string? Description { get; set; }
}
