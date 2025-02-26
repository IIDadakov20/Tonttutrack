using Microsoft.AspNetCore.Identity;
using Tonttutrack.Service.Contracts;
using Tonttutrack.DataAccess.Data.Models;
using Tonttutrack.Domain.DTOs.Request;
using Tonttutrack.Domain.DTOs.Response;
using Tonttutrack.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace Tonttutrack.Service.Services;

internal class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IRouteService _routeService;
    private readonly TonttutrackDbContext _context;

    public UserService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ICurrentUserService currentUserService,
        IRouteService routeService,
        TonttutrackDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _currentUserService = currentUserService;
        _routeService = routeService;
        _context = context;
    }

    public async Task<ErrorDTO> UpdateUserAsync(UserRequestDTO userInfo)
    {
        var result = new ErrorDTO();

        var user = (await _userManager.FindByEmailAsync(_currentUserService.CurrentUser.Email))!;

        if (await _userManager.FindByEmailAsync(userInfo.Email) != null ^ userInfo.Email == user.Email)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("Email", "User already exists with this email.");
        }

        if (await _userManager.FindByNameAsync(userInfo.Username) != null ^ userInfo.Username == user.UserName)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("Username", "User already exists with this username.");

        }

        if (result.ErrorMessage.Any())
        {
            return result;
        }

        user.Email = userInfo.Email;
        user.UserName = userInfo.Username;

        var identityResult = await _userManager.UpdateAsync(user);

        if (!identityResult.Succeeded)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("", "Problem occurred while updating your account");
            return result;
        }

        await _signInManager.RefreshSignInAsync(user);

        result.Succeeded = true;
        return result;
    }

    public async Task<ErrorDTO> ChangeUserPasswordAsync(UserPasswordRequestDTO passwordInfo)
    {
        var result = new ErrorDTO();

        var user = await _userManager.FindByEmailAsync(_currentUserService.CurrentUser.Email);

        var identityResult = await _userManager.ChangePasswordAsync(user!, passwordInfo.CurrentPassword, passwordInfo.NewPassword);

        if (!identityResult.Succeeded)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("", "Problem occurred while changing your password");
            return result;
        }

        result.Succeeded = true;
        return result;
    }

    public async Task<ErrorDTO> DeleteUserAsync(string password)
    {
        var result = new ErrorDTO();

        var user = await _userManager.FindByEmailAsync(_currentUserService.CurrentUser.Email);

        bool routeResult = await _routeService.DeleteUserRoutesAsync(user!.Id);

        if (!routeResult)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("", "Problem occurred while deleting your account");
            return result;
        }

        _context.Entry(user).State = EntityState.Detached;

        user = await _context.Users
        .Include(u => u.Routes)
        .FirstOrDefaultAsync(u => u.Id == user.Id);

        var identityResult = await _userManager.DeleteAsync(user!);

        if(!identityResult.Succeeded)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("", "Problem occurred while deleting your account");
            return result;
        }

        await _signInManager.SignOutAsync();

        result.Succeeded = true;
        return result;
    }
}