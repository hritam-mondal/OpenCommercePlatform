namespace OCP.API.DTOs;

public sealed class UpdateSubCategoryDto
{
    public string? Name { get; set; }
    public int? CategoryId { get; set; }
    public bool? IsActive { get; set; }
    public int? StoreId { get; set; }
}
