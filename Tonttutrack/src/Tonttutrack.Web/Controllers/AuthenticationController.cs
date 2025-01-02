using Microsoft.AspNetCore.Mvc;
using Tonttutrack.Service.Contracts;
using Tonttutrack.Domain.DTOs.Authentication;

namespace Tonttutrack.Web.Controllers;

public class AuthenticationController : Controller
{
	private readonly IAuthenticationService _userAuthService;

	public AuthenticationController(
		IAuthenticationService userAuthService)
	{
		_userAuthService = userAuthService;
	}

	public IActionResult Register()
	{
		return View("Register");
	}

	[HttpPost]
	public async Task<IActionResult> RegisterAsync(RegisterDTO registerInfo)
	{
        var identityResult = await _userAuthService.RegisterUserAsync(registerInfo);

        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.ErrorMessage)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
            return View("Register");
        }

        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            ModelState.AddModelError("", "Problem occurred while login to your account");
            return View("Login");
        }

        return RedirectToAction("Index", "Home");
    }

	public IActionResult Login()
	{
		return View("Login");
	}

	[HttpPost]
	public async Task<IActionResult> LoginAsync(LoginDTO loginInfo)
	{
        var identityResult = await _userAuthService.LoginUserAsync(loginInfo);

        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.ErrorMessage)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
            return View("Login");
        }

        if (User.Identity == null || !User.Identity.IsAuthenticated)
		{
			ModelState.AddModelError("", "Problem occurred while login to your account");
			return View("Login", loginInfo);
		}

		return RedirectToAction("Index", "Home");
	}

    public async Task<IActionResult> Signout()
	{
		await _userAuthService.UserSignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}