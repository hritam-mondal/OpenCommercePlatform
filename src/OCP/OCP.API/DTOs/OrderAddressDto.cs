namespace OCP.API.DTOs;

public sealed class OrderAddressDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Pin { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AddressType { get; set; }
    public string State { get; set; } = string.Empty;
}
