namespace OCP.API.DTOs;

public sealed class CreateDiscountDto
{
    public decimal? DiscountPer { get; set; }
    public decimal? DiscountAmt { get; set; }
    public bool? IsApplicable { get; set; }
    public string? Remarks { get; set; }
    public int? StoreId { get; set; }
}
