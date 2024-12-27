using Microsoft.AspNetCore.Mvc;
using Tonttutrack.Service.Contracts;
using Tonttutrack.Domain.DTOs.Authentication;

namespace Tonttutrack.Web.Controllers;

public class AuthenticationController : Controller
{
	private readonly IAuthenticationService _userAuthService;
	private readonly IUserService _userService;

	public AuthenticationController(
		IAuthenticationService userAuthService,
		IUserService userService)
	{
		_userAuthService = userAuthService;
		_userService = userService;
	}

	public IActionResult Register()
	{
		return View("Views/Account/Register.cshtml");
	}

	[HttpPost]
	public async Task<IActionResult> RegisterAsync(RegisterDTO registerInfo)
	{
		if (await _userService.CheckUserExistsByEmailAsync(registerInfo.Email))
		{
			ModelState.AddModelError("Email", "User already exists with that email.");
		}

		if (await _userService.CheckUserExistsByUsernameAsync(registerInfo.Username))
		{
			ModelState.AddModelError("Username", "User already exists with that username.");
		}

		if (!ModelState.IsValid)
		{
			return View("Views/Account/Register.cshtml");
		}

		var registrationResult = await _userService.CreateUserAsync(registerInfo);

		if (!registrationResult.Succeeded)
		{
			ModelState.AddModelError("", "Problem occurred while creating your profile");
			return View("Views/Account/Register.cshtml");
		}

		await _userAuthService.UserSignInAsync(registerInfo.Email);

		return RedirectToAction("Index", "Home");
	}

	public IActionResult Login()
	{
		return View("Views/Account/Login.cshtml");
	}

	[HttpPost]
	public async Task<IActionResult> LoginAsync(LoginDTO loginInfo)
	{
		bool areCredentialsValid = await _userAuthService.VerifyUserCredentialsAsync(loginInfo.Email, loginInfo.Password);

		if (!areCredentialsValid)
		{
			ModelState.AddModelError("", "Invalid email or password.");
			return View("Views/Account/Login.cshtml", loginInfo);
		}

		await _userAuthService.UserSignInAsync(loginInfo.Email);

		if(User.Identity == null || !User.Identity.IsAuthenticated)
		{
			ModelState.AddModelError("", "Problem occurred while login to your account");
			return View("Views/Account/Login.cshtml", loginInfo);
		}

		return RedirectToAction("Index", "Home");
	}

    public async Task<IActionResult> Signout()
	{
		await _userAuthService.UserSignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}