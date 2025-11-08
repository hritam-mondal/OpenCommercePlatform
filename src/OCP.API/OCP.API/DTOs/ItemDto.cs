namespace OCP.API.DTOs;

public sealed class ItemDto
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public string? ImageUrl { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public bool IsActive { get; set; }
}
