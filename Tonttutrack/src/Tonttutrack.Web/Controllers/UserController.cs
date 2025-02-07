using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tonttutrack.DataAccess.Data.Models;
using Tonttutrack.Domain.DTOs.Request;
using Tonttutrack.Domain.DTOs.Response;
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
        return HandleIdentityResult(identityResult);
    }

    [HttpPatch("updateUserPassword")]
    public async Task<IActionResult> UpdateUserPasswordAsync([FromBody]UserPasswordRequestDTO passwordInfo)
    {
        var identityResult = await _userService.ChangeUserPasswordAsync(passwordInfo);
        return HandleIdentityResult(identityResult);
    }

    private IActionResult HandleIdentityResult(ErrorDTO identityResult)
    {
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
