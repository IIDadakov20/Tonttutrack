using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Tonttutrack.Service.Contracts;
using Tonttutrack.Domain.DTOs.Response;

namespace Tonttutrack.Service.Services;

internal class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserResponseDTO CurrentUser
    { 
        get 
        {
            return new UserResponseDTO
            {
                Email = _httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value!,
                Username = _httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value!
            };
        } 
    }
}