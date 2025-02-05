using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tonttutrack.DataAccess.Data.Models;
using Tonttutrack.Domain.DTOs.Request;
using Tonttutrack.Service.Contracts;

namespace Tonttutrack.Web.Controllers;

[Route("user")]
[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(
        IUserService userService)
    {
        _userService = userService;
    }

    [HttpPut("updateUser")]
    public async Task<IActionResult> UpdateUserAsync([FromBody]UserRequestDTO userInfo)
    {
        var identityResult = await _userService.UpdateUserAsync(userInfo);

        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.ErrorMessage)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
            return BadRequest(ModelState);
        }

        return Json(true);
    }
}
