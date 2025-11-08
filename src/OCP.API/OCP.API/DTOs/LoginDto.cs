using System.ComponentModel.DataAnnotations;

namespace OCP.API.DTOs;

public class LoginDto
{
    [Required] [StringLength(254)] public string EmailOrUsername { get; set; } = string.Empty;

    [Required] [MinLength(8)] public string Password { get; set; } = string.Empty;
}
