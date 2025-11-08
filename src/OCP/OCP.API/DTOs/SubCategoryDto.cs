namespace OCP.API.DTOs;

public sealed class SubCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public bool IsActive { get; set; }
    public int? StoreId { get; set; }
}
