using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tonttutrack.DataAccess.Data;
using Tonttutrack.DataAccess.Data.Models;
using Tonttutrack.Domain.DTOs.Request;
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

    public async Task<List<RouteResponseDTO>> GetUserRoutesAsync(int pageNumber)
    {
        var result = new ErrorDTO();

        var user = (await _userManager.FindByEmailAsync(_currentUserService.CurrentUser.Email))!;

        var skip = (pageNumber - 1) * 3;

        return await _context.Routes
            .Where(r => r.UserId == user.Id)
            .OrderBy(r => r.Date)
            .Skip(skip)
            .Take(3)
            .Select(r => new RouteResponseDTO
            {
                Id = r.Id,
                Name = r.Name,
                Date = r.Date,
                Distance = r.Distance,
                Duration = r.Duration,
                RoutePoints = r.RoutePoints,
            })
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<ErrorDTO> CreateRouteAsync()
    {
        var result = new ErrorDTO();

        var user = (await _userManager.FindByEmailAsync(_currentUserService.CurrentUser.Email))!;

        Route newRoute = new Route
        {
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

    public async Task<ErrorDTO> UpdateRouteAsync(RouteRequestDTO route)
    {
        var result = new ErrorDTO();

        var currentRoute = await _context.Routes
            .Where(r => r.Id == route.Id)
            .Include(r => r.RoutePoints)
            .FirstAsync();

        double distance = currentRoute.Distance;
        TimeSpan duration = currentRoute.Duration;
        string name = currentRoute.Name;

        if (distance == 0)
        {
            distance = this.CalculateTotalDistance(currentRoute);
            duration = currentRoute.RoutePoints.Last().RecordedAt - currentRoute.RoutePoints.First().RecordedAt;
            duration = TimeSpan.FromSeconds(Math.Round(duration.TotalSeconds));
        }

        if (route.Name != "new")
        {
            name = route.Name;
        }

        var routeCreationResult = await _context.Routes
            .Where(r => r.Id == route.Id)
            .ExecuteUpdateAsync(r => r
                .SetProperty(r => r.Name, name)
                .SetProperty(r => r.Distance, distance)
                .SetProperty(r => r.Duration, duration)
            );

        if (routeCreationResult > 0)
        {
            result.Succeeded = true;
            return result;
        }

        result.Succeeded = false;
        result.ErrorMessage.Add("", "Problem occurred while saving your route");
        return result;
    }

    public async Task<ErrorDTO> DeleteRouteAsync(Guid routeId)
    {
        var result = new ErrorDTO();

        var route = await _context.Routes.FindAsync(routeId);

        if (route == null)
        {
            result.Succeeded = true;
            return result;
        }

        var deleteRoutePointsResult = await DeleteRoutePointsAsync(routeId);
        if (!deleteRoutePointsResult)
        {
            result.Succeeded = false;
            result.ErrorMessage.Add("", "Failed to delete route points.");
            return result;
        }

        var routeDeletionResult = await _context.Routes
            .Where(r => r.Id == routeId)
            .ExecuteDeleteAsync();

        if (routeDeletionResult > 0)
        {
            result.Succeeded = true;
            return result;
        }

        result.Succeeded = false;
        result.ErrorMessage.Add("", "Problem occurred while deleting a route");
        return result;
    }

    public async Task<bool> DeleteUserRoutesAsync(Guid userId)
    {
        var routes = await _context.Routes
            .Where(r => r.UserId == userId)
            .Select(r => r.Id)
            .ToListAsync();

        if (!routes.Any())
        {
            return true;
        }

        foreach (var routeId in routes)
        {
            var result = await DeleteRouteAsync(routeId);
            if (!result.Succeeded)
            {
                return false;
            }
        }

        return true;
    }

    public async Task<ErrorDTO> SaveRoutePointAsync(JsonElement data)
    {
        var result = new ErrorDTO();

        string route = data.GetProperty("route").GetString()!;
        var routePointJson = data.GetProperty("routePoint").ToString();
        var routePoint = JsonSerializer.Deserialize<RoutePointDTO>(routePointJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

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

    private async Task<bool> DeleteRoutePointsAsync(Guid routeId)
    {
        var hasRoutePoints = await _context.RoutePoints
            .AnyAsync(rp => rp.RouteId == routeId);

        if (!hasRoutePoints)
        {
            return true;
        }

        var routePointsDeletionResult = await _context.RoutePoints
            .Where(rp => rp.RouteId == routeId)
            .ExecuteDeleteAsync();

        if (routePointsDeletionResult > 0)
        {
            return true;
        }

        return false;
    }
}