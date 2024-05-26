using Microsoft.AspNetCore.Identity;
using Tonttutrack.SharedModels.DTO;

namespace Tonttutrack.BLL.Contracts;

public interface IUserService
{
    Task<IdentityResult> CreateUserAsync(RegisterDTO userInput);

    Task<UserDTO?> GetUserByEmailAsync(string email);
}