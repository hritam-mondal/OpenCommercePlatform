using System.ComponentModel.DataAnnotations;

namespace OCP.API.DTOs;

public class UpdateUserDto
{
    [Required] public int UserId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string LastName { get; set; } = string.Empty;

    [Phone] [StringLength(25)] public string? PhoneNumber { get; set; }
}
