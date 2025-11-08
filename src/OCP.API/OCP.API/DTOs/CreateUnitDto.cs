namespace OCP.API.DTOs;

public sealed class CreateUnitDto
{
    public string? UnitName { get; set; }
    public int? UnitOrder { get; set; }
    public decimal? ConversionRatio { get; set; }
}
