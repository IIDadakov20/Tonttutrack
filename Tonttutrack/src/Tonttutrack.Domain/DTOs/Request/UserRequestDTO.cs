using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.Domain.DTOs.Request;

public class UserRequestDTO
{
    [StringLength(25, MinimumLength = 2, ErrorMessage = "Username must be 2 - 25 characters long.")]
    public string Username { get; set; } = null!;

    [DataType(DataType.EmailAddress)]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    [StringLength(25, MinimumLength = 6, ErrorMessage = "Password must be 6 - 25 characters long.")]
    [RegularExpression(@"^(?=.*[a-zA-Z])[A-Za-z\d@$!%*?&]+$", ErrorMessage = "Password must contain at least one letter.")]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password doesn't match.")]
    public string ConfirmPassword { get; set; } = null!;
}