using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.PL.Models;

public class LoginViewModel
{
	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.EmailAddress)]
	[StringLength(100)]
	public string Email { get; set; } = string.Empty;

	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.Password)]
	[StringLength(25, MinimumLength = 6)]
	public string Password { get; set; } = string.Empty;
}