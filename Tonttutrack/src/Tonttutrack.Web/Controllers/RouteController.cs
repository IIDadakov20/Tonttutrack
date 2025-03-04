using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tonttutrack.Domain.DTOs.Request;
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

    [HttpPatch("updateRoute")]
    public async Task<IActionResult> UpdateRouteAsync([FromBody]RouteRequestDTO route)
    {
        var routeUpdateResult = await _routeService.UpdateRouteAsync(route);

        if (!routeUpdateResult.Succeeded)
        {
            return BadRequest();
        }

        return Ok();
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