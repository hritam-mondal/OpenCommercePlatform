namespace OCP.API.DTOs;

public sealed class AddCartItemDto
{
    public int CartId { get; set; }
    public int ItemId { get; set; }
    public int UnitId { get; set; }
    public int Quantity { get; set; }
    public decimal? Rate { get; set; }
}
