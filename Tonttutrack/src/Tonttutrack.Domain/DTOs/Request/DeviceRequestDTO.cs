using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.Domain.DTOs.Request;

public class DeviceRequestDTO
{
    [Required(ErrorMessage = "This field is required.")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "This field is required.")]
    public string PasswordHash { get; set; } = null!;

    [Required(ErrorMessage = "This field is required.")]
    [StringLength(17)]
    [Unicode(false)]
    public string Code { get; set; } = null!;
}