using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.Domain.DTOs.Authentication;

public class LoginDTO
{
	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.EmailAddress)]
	public string Email { get; set; } = null!;

	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.Password)]
	public string Password { get; set; } = null!;
}