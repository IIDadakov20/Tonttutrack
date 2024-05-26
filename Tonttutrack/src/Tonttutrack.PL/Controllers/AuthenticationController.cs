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
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> RegisterAsync(RegisterDTO userInput)
	{
		if (ModelState.IsValid)
		{
			if (await _userAuthService.CheckUserExistsByEmailAsync(userInput.Email))
			{
				ModelState.AddModelError("Email", "User already exists with that email.");
			}

			if (await _userAuthService.CheckUserExistsByUsernameAsync(userInput.Username))
			{
				ModelState.AddModelError("Username", "User already exists with that username.");
			}

			var registrationResult = await _userService.CreateUserAsync(userInput);

			if (!registrationResult.Succeeded)
			{
				ModelState.AddModelError("", "Problem occurred while creating your profile");
				return View(userInput);
			}

			return RedirectToAction("Index", "Home");
		}

		return View(userInput);
	}

	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
	public async Task<ActionResult<IActionResult>> LoginAsync(LoginDTO userInput)
	{
		if (ModelState.IsValid)
		{

			if (!await _userAuthService.CheckUserExistsByEmailAsync(userInput.Email))
			{
				ModelState.AddModelError("Email", "Account with this email is not found.");
			}

			if (!await _userAuthService.VerifyUserPasswordAsync(userInput.Email, userInput.Password))
			{
				ModelState.AddModelError("Password", "Incorrect password.");
			}

			var user = await _userService.GetUserByEmailAsync(userInput.Email);

			if (user != null)
			{
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Problem occurred while login to your profile");
            return View(userInput);
		}

		return View(userInput);
	}

	public IActionResult Logout()
	{
		return View();
	}
}