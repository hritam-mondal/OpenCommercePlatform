namespace OCP.API.DTOs;

public sealed class UpdateItemDto
{
    public int? SubCategoryId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public string? ImageUrl { get; set; }
    public string? CreatedBy { get; set; }
    public bool? IsActive { get; set; }
}
