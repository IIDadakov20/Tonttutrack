using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.PL.Models;

public class LoginViewModel
{
	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.EmailAddress)]
	public string Email { get; set; } = string.Empty;

	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.Password)]
	public string Password { get; set; } = string.Empty;
}