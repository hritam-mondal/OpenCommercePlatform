namespace OCP.API.DTOs;

public sealed class OrderItemInputDto
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public int UnitId { get; set; }
    public decimal Rate { get; set; }
    public decimal ItemPrice { get; set; }
    public int StoreId { get; set; }
    public decimal DiscountPer { get; set; }
    public decimal ActualRate { get; set; }
}
