using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[Table("menu", Schema = "navigation")]
public partial class Menu
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("display_name")]
    [StringLength(100)]
    public string DisplayName { get; set; } = null!;

    [InverseProperty("Menu")]
    public virtual ICollection<Page> Pages { get; set; } = new List<Page>();
}
