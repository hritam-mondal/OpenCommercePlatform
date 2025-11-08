namespace OCP.API.DTOs;

public class UserFilterDto
{
    public string? Query { get; set; }
    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
