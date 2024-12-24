using Microsoft.AspNetCore.Identity;
using Tonttutrack.Domain.DTOs.Authentication;

namespace Tonttutrack.Service.Contracts;

public interface IUserService
{
    Task<IdentityResult> CreateUserAsync(RegisterDTO userInput);

    //Task<UserDTO?> GetUserByEmailAsync(string email);
}