using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tonttutrack.Domain.DTOs.Request;
using Tonttutrack.Service.Contracts;

namespace Tonttutrack.Web.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(
        IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserAsync(UserRequestDTO user)
    {
        var identityResult = await _userService.UpdateUserAsync(user);

        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.ErrorMessage)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }

        return View("Register");
    }
}
