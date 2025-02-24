using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Tonttutrack.DataAccess.Data;
using Tonttutrack.DataAccess.Data.Models;
using Tonttutrack.Domain.DTOs.Response;
using Tonttutrack.Service.Contracts;

namespace Tonttutrack.Service.Services;

internal class RouteService : IRouteService
{
    private readonly TonttutrackDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<User> _userManager;

    public RouteService(
        TonttutrackDbContext context,
        ICurrentUserService currentUserService,
        UserManager<User> userManager)
    {
        _context = context;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

    public async Task<ErrorDTO> CreateRouteAsync()
    {
        var result = new ErrorDTO();

        var user = (await _userManager.FindByEmailAsync(_currentUserService.CurrentUser.Email))!;

        Route newRoute = new Route {
            UserId = user.Id,
            Name = $"{user.UserName}_{DateOnly.FromDateTime(DateTime.Now)}"
        };
        await _context.Routes.AddAsync(newRoute);
        var routeCreationResult = await _context.SaveChangesAsync();

        if (routeCreationResult > 0)
        {
            result.Succeeded = true;
            result.ErrorMessage.Add("", $"{newRoute.Id}");
            return result;
        }

        result.Succeeded = false;
        result.ErrorMessage.Add("", "Problem occurred while creating new route");
        return result;
    }

    public async Task<ErrorDTO> SaveRoutePointAsync(JsonElement data)
    {
        var result = new ErrorDTO();

        string route = data.GetProperty("route").GetString()!;
        var routePointJson = data.GetProperty("routePoint").ToString();
        var routePoint = JsonSerializer.Deserialize<RoutePointDTO>(routePointJson)!;

        RoutePoint newRoutePoint = new RoutePoint
        {
            Latitude = routePoint.Latitude,
            Longitude = routePoint.Longitude,
            CurrentSpeed = routePoint.CurrentSpeed,
            RouteId = Guid.Parse(route),
        };
        await _context.RoutePoints.AddAsync(newRoutePoint);
        var routePointSavedResult = await _context.SaveChangesAsync();

        if (routePointSavedResult > 0)
        {
            result.Succeeded = true;
            return result;
        }

        result.Succeeded = false;
        result.ErrorMessage.Add("", "Problem occurred while saving new route point");
        return result;
    }
}