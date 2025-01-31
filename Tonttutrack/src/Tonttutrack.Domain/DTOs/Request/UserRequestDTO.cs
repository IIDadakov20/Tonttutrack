using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.Domain.DTOs.Request;

public class UserRequestDTO
{
    [StringLength(25, MinimumLength = 2, ErrorMessage = "Username must be 2 - 25 characters long.")]
    public string Username { get; set; } = null!;

    [DataType(DataType.EmailAddress)]
    [StringLength(100)]
    public string Email { get; set; } = null!;
}