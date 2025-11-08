using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using OCP.API.Models;

namespace OCP.Database.Models;

[Table("roles", Schema = "security")]
[Index("Name", Name = "roles_name_key", IsUnique = true)]
public partial class Role
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<PageRole> PageRoles { get; set; } = new List<PageRole>();

    [InverseProperty("Role")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
