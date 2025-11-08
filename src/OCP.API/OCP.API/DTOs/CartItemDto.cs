namespace OCP.API.DTOs;

public sealed class CartItemDto
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int? ItemId { get; set; }
    public int? UnitId { get; set; }
    public int? ItemQuantity { get; set; }
    public decimal? Rate { get; set; }
    public decimal? ItemPrice { get; set; }
}
