namespace OCP.API.DTOs;

public sealed class CartDto
{
    public int Id { get; set; }
    public long? UserId { get; set; }
    public int? TotalItems { get; set; }
    public IReadOnlyList<CartItemDto> Items { get; set; } = new List<CartItemDto>();
}
