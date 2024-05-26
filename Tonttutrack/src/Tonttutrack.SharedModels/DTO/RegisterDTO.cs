using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.SharedModels.DTO;

public class RegisterDTO
{
	[Required(ErrorMessage = "This field is required.")]
	[StringLength(50, MinimumLength = 2)]
	public string Username { get; set; } = string.Empty;

	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.EmailAddress)]
	[StringLength(100)]
	public string Email { get; set; } = string.Empty;

	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.Password)]
	[StringLength(25, MinimumLength = 6)]
	public string Password { get; set; } = string.Empty;

	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.Password)]
	[StringLength(25, MinimumLength = 6)]
	[Compare("Password", ErrorMessage = "Password doesn't match.")]
	public string ConfirmPassword { get; set; } = string.Empty;
}