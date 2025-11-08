using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[Table("carts", Schema = "cart")]
public partial class Cart
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public long? UserId { get; set; }

    [Column("total_items")]
    public int? TotalItems { get; set; }

    [InverseProperty("Cart")]
    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    [ForeignKey("UserId")]
    [InverseProperty("Carts")]
    public virtual User? User { get; set; }
}
