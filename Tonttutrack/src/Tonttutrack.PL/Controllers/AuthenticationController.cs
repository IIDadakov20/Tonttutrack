using Microsoft.AspNetCore.Mvc;
using Tonttutrack.BLL.Contracts;
using Tonttutrack.BLL.DTO;
using Tonttutrack.PL.Models;

namespace Tonttutrack.PL.Controllers;

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
	public async Task<IActionResult> RegisterAsync(UserDTO userInput)
	{
		if (await _userAuthService.CheckUserExistsByEmailAsync(userInput.Email))
		{
			ModelState.AddModelError("Email", "User already exists with that email.");
		}

		if (await _userAuthService.CheckUserExistsByUsernameAsync(userInput.Username))
		{
			ModelState.AddModelError("Username", "User already exists with that username.");
		}

		if (!ModelState.IsValid)
		{
            return View("Views/Account/Register.cshtml");
        }

        var registrationResult = await _userService.CreateUserAsync(userInput);

		if (!registrationResult.Succeeded)
		{
			ModelState.AddModelError("", "Problem occurred while creating your profile");
			return View("Views/Account/Register.cshtml");
        }

        await _userAuthService.UserSignInAsync(userInput.Email);

        return RedirectToAction("Index", "Home");
	}

	public IActionResult Login()
	{
		return View("Views/Account/Login.cshtml");
	}

	[HttpPost]
	public async Task<IActionResult> LoginAsync(LoginViewModel userInput)
	{
		bool userExists = await _userAuthService.CheckUserExistsByEmailAsync(userInput.Email);
		bool passwordIsValid = false;

		if (userExists)
		{
			passwordIsValid = await _userAuthService.VerifyUserPasswordAsync(userInput.Email, userInput.Password);
		}

		if (!passwordIsValid)
		{
			ModelState.AddModelError("", "Invalid email or password.");
			return View("Views/Account/Login.cshtml", userInput);
		}

		await _userAuthService.UserSignInAsync(userInput.Email);

		if(User.Identity == null || !User.Identity.IsAuthenticated)
		{
			ModelState.AddModelError("", "Problem occurred while login to your account");
			return View("Views/Account/Login.cshtml", userInput);
		}

		return RedirectToAction("Index", "Home");
	}

    public async Task<IActionResult> Logout()
	{
		await _userAuthService.UserSignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}