using Microsoft.AspNetCore.Identity;
using Tonttutrack.BLL.DTO;

namespace Tonttutrack.BLL.Contracts;

public interface IUserService
{
    Task<IdentityResult> CreateUserAsync(UserDTO userInput);

    Task<UserDTO?> GetUserByEmailAsync(string email);
}