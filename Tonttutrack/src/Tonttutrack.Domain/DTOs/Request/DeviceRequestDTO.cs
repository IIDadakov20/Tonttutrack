using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.Domain.DTOs.Request;

public class DeviceRequestDTO
{
    [Required(ErrorMessage = "This field is required.")]
    public string Code { get; set; } = null!;

    [Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}