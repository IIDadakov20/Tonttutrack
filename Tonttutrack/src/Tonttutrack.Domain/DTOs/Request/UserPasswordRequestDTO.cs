using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.Domain.DTOs.Request;

public class UserPasswordRequestDTO
{
    [Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.Password)]
    [StringLength(25, MinimumLength = 6, ErrorMessage = "Password must be 6 - 25 characters long.")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)[A-Za-z\d@_!]*$", ErrorMessage = "Password must contain at least one letter and digit.")]
    public string NewPassword { get; set; } = null!;

    [Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Password doesn't match.")]
    public string ConfirmPassword { get; set; } = null!;
}
