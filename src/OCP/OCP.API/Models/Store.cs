using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OCP.Database.Models;

namespace OCP.API.Models;

[Table("stores", Schema = "store")]
public partial class Store
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(200)]
    public string? Name { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("image_url")]
    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [Column("company_name")]
    [StringLength(300)]
    public string? CompanyName { get; set; }

    [Column("company_address")]
    public string? CompanyAddress { get; set; }

    [Column("gstn")]
    [StringLength(50)]
    public string? Gstn { get; set; }

    [InverseProperty("Store")]
    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    [InverseProperty("Store")]
    public virtual ICollection<ItemUnit> ItemUnits { get; set; } = new List<ItemUnit>();

    [InverseProperty("Store")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("Store")]
    public virtual ICollection<OrderNotification> OrderNotifications { get; set; } = new List<OrderNotification>();

    [InverseProperty("Store")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Store")]
    public virtual ICollection<Pincode> Pincodes { get; set; } = new List<Pincode>();

    [InverseProperty("Store")]
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    [InverseProperty("Store")]
    public virtual ICollection<StoreUser> StoreUsers { get; set; } = new List<StoreUser>();

    [InverseProperty("Store")]
    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
}
