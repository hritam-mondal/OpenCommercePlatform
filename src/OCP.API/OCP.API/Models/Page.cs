using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using OCP.API.Models;

namespace OCP.Database.Models;

[Table("pages", Schema = "navigation")]
public partial class Page
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("menu_id")]
    public int? MenuId { get; set; }

    [Column("display_name")]
    [StringLength(200)]
    public string? DisplayName { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [ForeignKey("MenuId")]
    [InverseProperty("Pages")]
    public virtual Menu? Menu { get; set; }

    [InverseProperty("Page")]
    public virtual ICollection<PageRole> PageRoles { get; set; } = new List<PageRole>();
}
