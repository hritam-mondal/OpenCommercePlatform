namespace OCP.API.DTOs;

public sealed class OrderSummaryDto
{
    public string OrderId { get; set; } = string.Empty;
    public string? OrderName { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal? FinalPrice { get; set; }
    public int UserId { get; set; }
    public int? StoreId { get; set; }
    public string? OrderStatus { get; set; }
    public string? PaymentStatus { get; set; }
}
