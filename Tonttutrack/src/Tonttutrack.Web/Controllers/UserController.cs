using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

    /*[HttpPost]
    public async IActionResult Update([FromBody] UserDTO user)
    {
        if (await _userService.CheckUserExistsByEmailAsync(user.Email, User.FindFirstValue(ClaimTypes.Email)))
        {
            ModelState.AddModelError("Email", "User already exists with that email.");
        }

        if (await _userService.CheckUserExistsByUsernameAsync(user.Username, User.Identity.Name))
        {
            ModelState.AddModelError("Username", "User already exists with that username.");
        }

        if (!ModelState.IsValid)
        {
            return View("Views/Account/Register.cshtml");
        }
    }*/
}
