using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Tonttutrack.Service.Contracts;
using Tonttutrack.DataAccess.Data.Models;
using Tonttutrack.Domain.DTOs.Response;

namespace Tonttutrack.Service.Services;

internal class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        CurrentUser.Username = httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value!;
        CurrentUser.Email = httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type != ClaimTypes.Email)?.Value!;
    }

    public UserResponseDTO CurrentUser { get; } = null!;
}