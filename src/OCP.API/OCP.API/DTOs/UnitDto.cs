namespace OCP.API.DTOs;

public sealed class UnitDto
{
    public int Id { get; set; }
    public string? UnitName { get; set; }
    public int? UnitOrder { get; set; }
    public decimal? ConversionRatio { get; set; }
}
