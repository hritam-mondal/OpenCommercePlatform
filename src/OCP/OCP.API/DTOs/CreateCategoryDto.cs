namespace OCP.API.DTOs;

public sealed class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
