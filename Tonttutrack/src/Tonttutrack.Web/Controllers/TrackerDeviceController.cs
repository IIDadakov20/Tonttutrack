using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tonttutrack.Service.Contracts;
using Tonttutrack.Domain.DTOs.Response;
using System.Web;
using Tonttutrack.Domain.DTOs.Request;

namespace Tonttutrack.Web.Controllers;

[Authorize]
[Route("trackerDevice")]
public class TrackerDeviceController : Controller
{
    private readonly IDeviceCommunicationService _deviceCommunicationService;

    public TrackerDeviceController(IDeviceCommunicationService deviceCommunicationService)
    {
        _deviceCommunicationService = deviceCommunicationService;
    }

    [HttpPost("connectDevice")]
    public async Task<IActionResult> ConnectDeviceAsync([FromBody] DeviceConnectionRequestDTO deviceInfo)
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

    [HttpGet("readRoutePoint")]
    public async Task<IActionResult> ReadFromDeviceAsync([FromQuery] string deviceCode)
    {
        var code = HttpUtility.UrlDecode(deviceCode);

        RoutePointDTO? routePoint = _deviceCommunicationService.GetRoutePointData(code);

        if (routePoint == null)
        {
            return BadRequest(new { message = "Unable to read route point. Please ensure your device is connected." });
        }

        return Ok(new
        {
            routePoint.Latitude,
            routePoint.Longitude,
            routePoint.CurrentSpeed
        });
    }
}