namespace OCP.API.DTOs;

public sealed class UpdateUnitDto
{
    public string? UnitName { get; set; }
    public int? UnitOrder { get; set; }
    public decimal? ConversionRatio { get; set; }
}
