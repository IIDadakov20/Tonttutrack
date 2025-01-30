using Microsoft.AspNetCore.Identity;
using Tonttutrack.Service.Contracts;
using Tonttutrack.DataAccess.Data.Models;
using AutoMapper;
using Tonttutrack.Domain.DTOs.Request;
using Tonttutrack.Domain.DTOs.Response;

namespace Tonttutrack.Service.Services;

internal class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UserService(
        UserManager<User> userManager,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorDTO> UpdateUserAsync(UserRequestDTO userInfo)
    {
        var result = new ErrorDTO();

        if (await _userManager.FindByEmailAsync(userInfo.Email) != null ^ userInfo.Email == _currentUserService.CurrentUser.Email)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("Email", "User already exists with this email.");
        }

        if (await _userManager.FindByNameAsync(userInfo.Username) != null ^ userInfo.Username == _currentUserService.CurrentUser.Username)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("Username", "User already exists with this username.");
        }

        if (result.ErrorMessage.Any())
        {
            return result;
        }

        var user = _mapper.Map<User>(userInfo);
        var identityResult = await _userManager.UpdateAsync(user);

        if (!identityResult.Succeeded)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("", "Problem occurred while updating your account");
            return result;
        }

        result.Succeeded = true;
        return result;
    }
}