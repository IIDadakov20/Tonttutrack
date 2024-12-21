using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tonttutrack.Service.Contracts;
using Tonttutrack.Service.DTO;
using Tonttutrack.Web.Models;

namespace Tonttutrack.Web.Controllers;

[Authorize]
[Route("trackerDevice")]
public class TrackerDeviceController : Controller
{
    private readonly IDeviceCommunicationService _deviceCommunicationService;
    private readonly IDeviceService _deviceService;

    public TrackerDeviceController(
        IDeviceCommunicationService deviceCommunicationService,
        IDeviceService deviceService)
    {
        _deviceCommunicationService = deviceCommunicationService;
        _deviceService = deviceService;
    }

    [HttpPost("connectDevice")]
    public async Task<IActionResult> ConnectDeviceAsync([FromBody] DeviceConnectionViewModel userInput)
    {
        bool areCredentialsValid = await _deviceService.VerifyDeviceCredentialsAsync(userInput.Code, userInput.Password);

        if (!areCredentialsValid)
        {
            return BadRequest(new { message = "Invalid device code or password." });
        }

        bool deviceIsConnected = await _deviceCommunicationService.ConnectToBrokerAsync(userInput.Code);

        if (!deviceIsConnected)
        {
            return BadRequest(new {message = "Problem occurred while connecting to your device." });
        }

        string? deviceName = await _deviceService.FetchConnectedDeviceName(userInput.Code);

        var response = new
        {
            Success = true,
            DeviceName = deviceName
        };

        return Json(response);
    }

    [HttpGet("readRoutePoint")]
    public async Task<IActionResult> ReadFromDeviceAsync()
    {
        RoutePointDTO? routePoint = _deviceCommunicationService.GetRoutePointData();

        if (routePoint == null)
        {
            return Json(new { message = "No Data" });
        }

        return Json(new
        {
            routePoint.Latitude,
            routePoint.Longitude,
            routePoint.CurrentSpeed
        });
    }

    /*[HttpDelete("disconnectDevice")]
    public async Task<IActionResult> DisconnectDeviceAsync()
    {
        bool deviceIsDisconnected = await _deviceCommunicationService.DisconnectDeviceAsync();

        if (!deviceIsDisconnected)
        {
            return BadRequest(new { message = "Problem occurred while disconnecting your device." });
        }

        return Json(true);
    }*/
}