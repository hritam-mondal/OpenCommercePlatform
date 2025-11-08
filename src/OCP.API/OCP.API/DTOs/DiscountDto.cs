namespace OCP.API.DTOs;

public sealed class DiscountDto
{
    public int Id { get; set; }
    public decimal? DiscountPer { get; set; }
    public decimal? DiscountAmt { get; set; }
    public bool? IsApplicable { get; set; }
    public bool? IsDeleted { get; set; }
    public string? Remarks { get; set; }
    public int? StoreId { get; set; }
}
