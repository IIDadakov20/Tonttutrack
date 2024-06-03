using Microsoft.AspNetCore.Mvc;
using Tonttutrack.BLL.Contracts;
using Tonttutrack.SharedModels.DTO;

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
	public async Task<IActionResult> RegisterAsync(RegisterDTO userInput)
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
            return View("Views/Account/Register.cshtml", userInput);
        }

        var registrationResult = await _userService.CreateUserAsync(userInput);

		if (!registrationResult.Succeeded)
		{
			ModelState.AddModelError("", "Problem occurred while creating your profile");
			return View("Views/Account/Register.cshtml", userInput);
        }

		return RedirectToAction("Index", "Home");
	}

	public IActionResult Login()
	{
		return View("Views/Account/Login.cshtml");
	}

	[HttpPost]
	public async Task<ActionResult<LoginDTO>> LoginAsync(LoginDTO userInput)
	{
        bool userExists = await _userAuthService.CheckUserExistsByEmailAsync(userInput.Email);
        bool passwordIsValid = false;

        if (userExists)
        {
            passwordIsValid = await _userAuthService.VerifyUserPasswordAsync(userInput.Email, userInput.Password);
        }

        if (!userExists || !passwordIsValid)
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View("Views/Account/Login.cshtml", userInput);
        }

        var user = await _userService.GetUserByEmailAsync(userInput.Email);

		if (user == null)
		{
            ModelState.AddModelError("", "Problem occurred while login to your account");
            return View("Views/Account/Login.cshtml", userInput);
		}

        return RedirectToAction("Index", "Home");
    }

	public IActionResult Logout()
	{
		return View();
	}
}