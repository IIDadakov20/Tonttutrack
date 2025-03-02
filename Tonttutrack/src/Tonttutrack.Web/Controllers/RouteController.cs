using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tonttutrack.Service.Contracts;

namespace Tonttutrack.Web.Controllers;

[Authorize]
[Route("route")]
public class RouteController : Controller
{
    private readonly IRouteService _routeService;

    public RouteController(IRouteService routeService)
    {  
        _routeService = routeService;
    }

    [HttpPost("createRoute")]
    public async Task<IActionResult> CreateRouteAsync()
    {
        var routeCreationResult = await _routeService.CreateRouteAsync();

        if (!routeCreationResult.Succeeded)
        {
            return BadRequest();
        }

        return Json(routeCreationResult.ErrorMessage.Values);
    }

    [HttpGet("getRoutes")]
    public async Task<IActionResult> GetUserRoutesAsync([FromQuery]int pageNumber)
    {
        var routes = await _routeService.GetUserRoutesAsync(pageNumber);

        if (!routes.Any())
        {
            return BadRequest(new { message = "No routes found" });
        }

        return Json(routes);
    }

    [HttpPost("saveRoutePoint")]
    public async Task<IActionResult> ReadFromDeviceAsync([FromBody] JsonElement data)
    {
        var routePointSaved = await _routeService.SaveRoutePointAsync(data);

        if (!routePointSaved.Succeeded)
        {
            return BadRequest();
        }

        return Json(true);
    }
}