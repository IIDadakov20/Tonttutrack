using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tonttutrack.Service.Contracts;
using Tonttutrack.Domain.DTOs.Response;
using System.Text.Json;
using System.Web;
using Tonttutrack.Domain.DTOs.Request;

namespace Tonttutrack.Web.Controllers;

[Authorize]
[Route("trackerDevice")]
public class TrackerDeviceController : Controller
{
    private readonly IDeviceCommunicationService _deviceCommunicationService;
    private readonly IDeviceService _deviceService;
    private readonly IRouteService _routeService;

    public TrackerDeviceController(
        IDeviceCommunicationService deviceCommunicationService,
        IDeviceService deviceService,
        IRouteService routeService)
    {
        _deviceCommunicationService = deviceCommunicationService;
        _deviceService = deviceService;
        _routeService = routeService;
    }

    [HttpPost("connectDevice")]
    public async Task<IActionResult> ConnectDeviceAsync([FromBody] DeviceRequestDTO deviceInfo)
    {
        var deviceIsConnected = await _deviceCommunicationService.ConnectToBrokerAsync(deviceInfo);

        if (!deviceIsConnected.Succeeded)
        {
            return BadRequest(new { message = deviceIsConnected.ErrorMessage.Values });
        }

        return Ok(deviceIsConnected.ErrorMessage.Values);
    }

    [HttpDelete("disconnectDevice")]
    public async Task<IActionResult> DisconnectDeviceAsync([FromBody] string deviceCode)
    {
        var code = HttpUtility.UrlDecode(deviceCode);

        bool deviceIsDisconnected = await _deviceCommunicationService.DisconnectFromBrokerAsync(code);

        if (!deviceIsDisconnected)
        {
            return BadRequest(new { message = "Problem occurred while disconnecting your device." });
        }

        return Ok();
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

    [HttpGet("readRoutePoint")]
    public async Task<IActionResult> ReadFromDeviceAsync([FromQuery] string deviceCode)
    {
        var code = HttpUtility.UrlDecode(deviceCode);

        RoutePointDTO? routePoint = _deviceCommunicationService.GetRoutePointData(code);

        if (routePoint == null)
        {
            return BadRequest(new { message = "No data" });
        }

        return Json(new
        {
            routePoint.Latitude,
            routePoint.Longitude,
            routePoint.CurrentSpeed
        });
    }
}