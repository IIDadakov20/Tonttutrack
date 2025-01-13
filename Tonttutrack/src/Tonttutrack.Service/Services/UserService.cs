using Microsoft.AspNetCore.Identity;
using Tonttutrack.Service.Contracts;
using Tonttutrack.DataAccess.Data.Models;
using AutoMapper;
using Tonttutrack.Domain.DTOs.Request;

namespace Tonttutrack.Service.Services;

internal class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<User> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<bool> CheckUserExistsByEmailAsync(string email, string? currentEmail = null)
    {
        return await _userManager.FindByEmailAsync(email) != null ^ email == currentEmail;
    }

    public async Task<bool> CheckUserExistsByUsernameAsync(string username, string? currentUsername = null)
    {
        return await _userManager.FindByNameAsync(username) != null ^ username == currentUsername;
    }

    public async Task<IdentityResult> UpdateUserAsync(UserDTO userInfo)
    {
        var user = _mapper.Map<User>(userInfo);
        var identityResult = await _userManager.UpdateAsync(user);

        return identityResult;
    }
}