using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.Domain.DTOs.Authentication;

public class RegisterDTO
{
	[Required(ErrorMessage = "This field is required.")]
	[StringLength(25, MinimumLength = 2, ErrorMessage = "Username must be 2 - 25 characters long.")]
	public string Username { get; set; } = null!;

	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.EmailAddress)]
	[RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
	[StringLength(100)]
	public string Email { get; set; } = null!;

	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.Password)]
	[StringLength(25, MinimumLength = 6, ErrorMessage = "Password must be 6 - 25 characters long.")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)[A-Za-z\d@$!%*?&]+$", ErrorMessage = "Password must contain at least one letter and digit.")]
	public string Password { get; set; } = null!;

	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.Password)]
	[Compare("Password", ErrorMessage = "Password doesn't match.")]
	public string ConfirmPassword { get; set; } = null!;
}