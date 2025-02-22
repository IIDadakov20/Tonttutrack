using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tonttutrack.Service.Contracts;
using Tonttutrack.Domain.DTOs.Response;
using Tonttutrack.Web.Models;

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

        string? deviceName = await _deviceService.FetchConnectedDeviceNameAsync(userInput.Code);

        var response = new
        {
            Success = true,
            DeviceName = deviceName
        };

        return Json(response);
    }

    [HttpPost("createRoute")]
    public async Task<IActionResult> CreateRoute()
    {
        var routeCreationResult = await _routeService.CreateRouteAsync();

        if (!routeCreationResult.Succeeded)
        {
            return BadRequest();
        }

        return Json(true);
    }

    [HttpGet("readRoutePoint")]
    public async Task<IActionResult> ReadFromDeviceAsync([FromQuery] string deviceCode)
    {
        RoutePointDTO? routePoint = _deviceCommunicationService.GetRoutePointData(deviceCode);

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