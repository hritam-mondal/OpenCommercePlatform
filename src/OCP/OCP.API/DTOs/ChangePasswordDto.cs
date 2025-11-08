using System.ComponentModel.DataAnnotations;

namespace OCP.API.DTOs;

public class ChangePasswordDto
{
    [Required]
    [MinLength(8)]
    public string OldPassword { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;
}
