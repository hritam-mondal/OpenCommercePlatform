namespace OCP.API.DTOs;

public sealed class PincodeDto
{
    public string Pin { get; set; } = string.Empty;
    public string? DeliverDay { get; set; }
    public int StoreId { get; set; }
}
