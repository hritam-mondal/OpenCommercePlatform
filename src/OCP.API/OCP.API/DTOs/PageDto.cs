namespace OCP.API.DTOs;

public sealed class PageDto
{
    public int Id { get; set; }
    public int? MenuId { get; set; }
    public string? DisplayName { get; set; }
    public bool IsDeleted { get; set; }
}
