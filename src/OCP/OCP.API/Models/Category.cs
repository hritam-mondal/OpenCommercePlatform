using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCP.API.Models;

[Table("categories", Schema = "catalog")]
public partial class Category
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("is_active")]
    public bool IsActive { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
}
