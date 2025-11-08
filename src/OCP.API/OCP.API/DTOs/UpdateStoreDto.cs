namespace OCP.API.DTOs;

public sealed class UpdateStoreDto
{
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public string? Gstn { get; set; }
}
