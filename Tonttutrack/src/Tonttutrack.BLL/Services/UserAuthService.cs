using Microsoft.AspNetCore.Identity;
using Tonttutrack.BLL.Contracts;
using Tonttutrack.DAL.Data.Models;

namespace Tonttutrack.BLL.Services;

internal class UserAuthService : IUserAuthService
{
    private readonly UserManager<User> _userManager;

    public UserAuthService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> CheckUserExistsByEmail(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<bool> CheckUserExistsByUsername(string username)
    {
        return await _userManager.FindByNameAsync(username) != null;
    }

    public async Task<bool> VerifyUserPassword(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return false;

        return await _userManager.CheckPasswordAsync(user, password);
    }
}