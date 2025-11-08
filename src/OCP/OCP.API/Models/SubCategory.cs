using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCP.API.Models;

[Table("sub_categories", Schema = "catalog")]
public partial class SubCategory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("store_id")]
    public int? StoreId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("SubCategories")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("SubCategory")]
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    [InverseProperty("SubCategory")]
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    [ForeignKey("StoreId")]
    [InverseProperty("SubCategories")]
    public virtual Store? Store { get; set; }
}
