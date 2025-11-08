namespace OCP.API.DTOs;

public sealed class StoreDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public string? Gstn { get; set; }
}
