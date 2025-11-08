using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[Table("items", Schema = "catalog")]
public partial class Item
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("sub_category_id")]
    public int SubCategoryId { get; set; }

    [Column("name")]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    [Column("short_description")]
    [StringLength(300)]
    public string? ShortDescription { get; set; }

    [Column("image_url")]
    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [Column("created_on")]
    public DateTime CreatedOn { get; set; }

    [Column("created_by")]
    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    [InverseProperty("Item")]
    public virtual ICollection<ItemUnit> ItemUnits { get; set; } = new List<ItemUnit>();

    [InverseProperty("Item")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("Item")]
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    [ForeignKey("SubCategoryId")]
    [InverseProperty("Items")]
    public virtual SubCategory SubCategory { get; set; } = null!;
}
