using Microsoft.AspNetCore.Identity;
using Tonttutrack.BLL.Contracts;
using Tonttutrack.DAL.Data.Models;

namespace Tonttutrack.BLL.Services;

internal class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthenticationService(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> CheckUserExistsByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<bool> CheckUserExistsByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username) != null;
    }

    public async Task<bool> VerifyUserCredentialsAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return false;

        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task UserSignInAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null)
            await _signInManager.SignInAsync(user, false);
    }

    public async Task UserSignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}