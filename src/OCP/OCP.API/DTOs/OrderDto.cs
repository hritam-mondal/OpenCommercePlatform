namespace OCP.API.DTOs;

public sealed class OrderDto
{
    public string OrderId { get; set; } = string.Empty;
    public string? OrderName { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public int UserId { get; set; }
    public string? OrderNote { get; set; }
    public string? OrderStatus { get; set; }
    public string? PaymentStatus { get; set; }
    public bool IsActive { get; set; }
    public string? PaymentMode { get; set; }
    public int? StoreId { get; set; }
    public decimal? FinalPrice { get; set; }
    public decimal? OverallDiscountPer { get; set; }
    public List<OrderDetailLineDto> Lines { get; set; } = new();
    public OrderAddressDto? Address { get; set; }
}
