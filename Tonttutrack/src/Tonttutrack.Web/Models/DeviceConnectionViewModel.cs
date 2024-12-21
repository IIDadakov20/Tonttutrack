using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.Web.Models;

public class DeviceConnectionViewModel
{
    [Required(ErrorMessage = "This field is required.")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
