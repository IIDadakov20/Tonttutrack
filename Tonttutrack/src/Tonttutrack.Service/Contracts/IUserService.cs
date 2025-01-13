using Microsoft.AspNetCore.Identity;
using Tonttutrack.Domain.DTOs.Authentication;
using Tonttutrack.Domain.DTOs.Request;

namespace Tonttutrack.Service.Contracts;

public interface IUserService
{
    Task<bool> CheckUserExistsByEmailAsync(string email, string? currentEmail = null);

    Task<bool> CheckUserExistsByUsernameAsync(string username, string? currentUsername = null);

    Task<IdentityResult> UpdateUserAsync(UserDTO userInfo);
}