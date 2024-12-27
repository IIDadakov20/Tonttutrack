using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Tonttutrack.Service.Contracts;
using Tonttutrack.DataAccess.Data.Models;

namespace Tonttutrack.Service.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public User? CurrentUser
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            return _userManager.FindByIdAsync(userId!).Result;
        }
    }
}