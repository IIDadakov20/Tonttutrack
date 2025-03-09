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

        return Ok(routeCreationResult.ErrorMessage.Values);
    }

    [HttpPatch("updateRoute")]
    public async Task<IActionResult> UpdateRouteAsync([FromBody]RouteRequestDTO route)
    {
        var routeUpdateResult = await _routeService.UpdateRouteAsync(route);

        if (!routeUpdateResult.Succeeded)
        {
            return BadRequest(new { message = "Problem occured during route update" });
        }

        return Ok(true);
    }

    [HttpGet("getRoutesNumber")]
    public async Task<IActionResult> GetUserRoutesNumberAsync()
    {
        int routesNumber = await _routeService.GetUserRoutesNumberAsync();

        if (routesNumber == 0)
        {
            return BadRequest(new { message = "No routes found" });
        }

        return Ok(routesNumber);
    }

    [HttpGet("getRoutes")]
    public async Task<IActionResult> GetUserRoutesAsync([FromQuery]int pageNumber)
    {
        var routes = await _routeService.GetUserRoutesAsync(pageNumber);

        if (!routes.Any())
        {
            return BadRequest(new { message = "No routes found" });
        }

        return Ok(routes);
    }

    [HttpDelete("deleteRoute")]
    public async Task<IActionResult> DeleteeRouteAsync([FromBody] Guid routeId)
    {
        var routeDeleteResult = await _routeService.DeleteRouteAsync(routeId);

        if (!routeDeleteResult.Succeeded)
        {
            return BadRequest(new { message = "Problem occurred during route deletion" });
        }

        return Ok(true);
    }

    [HttpPost("saveRoutePoint")]
    public async Task<IActionResult> SaveRoutePointAsync([FromBody] JsonElement data)
    {
        var routePointSaved = await _routeService.SaveRoutePointAsync(data);

        if (!routePointSaved.Succeeded)
        {
            return BadRequest(new { message = "Problem occured with route point saving" });
        }

        return Ok(new { message = "Route point saved" });
    }
}