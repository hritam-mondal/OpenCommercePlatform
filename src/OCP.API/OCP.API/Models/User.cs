using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCP.API.Models;

[Table("users", Schema = "users")]
[Index("Email", Name = "users_email_key", IsUnique = true)]
[Index("Username", Name = "users_username_key", IsUnique = true)]
[Index("Uuid", Name = "users_uuid_key", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("user_id")]
    public long UserId { get; set; }

    [Column("uuid")]
    public Guid Uuid { get; set; }

    [Column("username")]
    [StringLength(50)]
    public string Username { get; set; } = null!;

    [Column("first_name")]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Column("email")]
    [StringLength(254)]
    public string Email { get; set; } = null!;

    [Column("phone_number")]
    [StringLength(25)]
    public string? PhoneNumber { get; set; }

    [Column("country_code")]
    [StringLength(2)]
    public string? CountryCode { get; set; }

    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Column("password_salt")]
    public string? PasswordSalt { get; set; }

    [Column("password_reset_token")]
    public Guid? PasswordResetToken { get; set; }

    [Column("reset_token_expires_at")]
    public DateTime? ResetTokenExpiresAt { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("is_admin")]
    public bool IsAdmin { get; set; }

    [Column("is_confirmed")]
    public bool IsConfirmed { get; set; }

    [Column("email_confirm_token")]
    public Guid? EmailConfirmToken { get; set; }

    [Column("email_confirm_token_expires")]
    public DateTime? EmailConfirmTokenExpires { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("last_login_at")]
    public DateTime? LastLoginAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    [InverseProperty("User")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("User")]
    public virtual ICollection<OrderNotification> OrderNotifications { get; set; } = new List<OrderNotification>();

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("ApplicationUser")]
    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();

    [InverseProperty("User")]
    public virtual ICollection<StoreUser> StoreUsers { get; set; } = new List<StoreUser>();

    [InverseProperty("User")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
