using Microsoft.AspNetCore.Identity;
using Tonttutrack.Service.Contracts;
using Tonttutrack.DataAccess.Data.Models;
using Tonttutrack.Domain.DTOs.Authentication;
using Tonttutrack.Domain.DTOs.Response;
using AutoMapper;

namespace Tonttutrack.Service.Services;

internal class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;

    public AuthenticationService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public async Task<ErrorDTO> RegisterUserAsync(RegisterDTO registerInfo)
    {
        var result = new ErrorDTO();

        if (await _userManager.FindByEmailAsync(registerInfo.Email) != null)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("Email", "User already exists with this email.");
        }

        if (await _userManager.FindByNameAsync(registerInfo.Username) != null)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("Username", "User already exists with this username.");
        }

        if (result.ErrorMessage.Any())
        {
            return result;
        }

        var user = _mapper.Map<User>(registerInfo);
        var identityResult = await _userManager.CreateAsync(user, registerInfo.Password);

        if (!identityResult.Succeeded)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("", "Problem occurred while creating your account");
            return result;
        }

        await _signInManager.SignInAsync(user, false);

        result.Succeeded = true;
        return result;
    }

    public async Task<ErrorDTO> LoginUserAsync(LoginDTO loginInfo)
    {
        var result = new ErrorDTO();

        var user = await _userManager.FindByEmailAsync(loginInfo.Email);

        if (user == null)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("", "User not found");
            return result;
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginInfo.Password);

        if (!isPasswordValid)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("", "Invalid email or password.");
            return result;
        }

        await _signInManager.SignInAsync(user, false);

        result.Succeeded = true;
        return result;
    }

    public async Task UserSignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}