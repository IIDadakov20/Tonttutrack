using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.SharedModels.DTO;

public class LoginDTO
{
	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.EmailAddress)]
	[StringLength(100)]
	public string Email { get; set; } = string.Empty;

	[Required(ErrorMessage = "This field is required.")]
	[DataType(DataType.Password)]
	public string Password { get; set; } = string.Empty;
}