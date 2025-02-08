using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.Domain.DTOs.Request;

public class UserRequestDTO
{
    [Required(ErrorMessage = "This field is required.")]
    [StringLength(25, MinimumLength = 2, ErrorMessage = "Username must be 2 - 25 characters long.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
    [StringLength(100)]
    public string Email { get; set; } = null!;
}