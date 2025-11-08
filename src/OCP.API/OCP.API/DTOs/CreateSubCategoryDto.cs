namespace OCP.API.DTOs;

public sealed class CreateSubCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public bool IsActive { get; set; } = true;
    public int? StoreId { get; set; }
}
