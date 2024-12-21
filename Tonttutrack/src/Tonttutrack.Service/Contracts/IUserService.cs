using Microsoft.AspNetCore.Identity;
using Tonttutrack.Service.DTO;

namespace Tonttutrack.Service.Contracts;

public interface IUserService
{
    Task<IdentityResult> CreateUserAsync(UserDTO userInput);

    Task<UserDTO?> GetUserByEmailAsync(string email);
}