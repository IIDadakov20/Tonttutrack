using Microsoft.AspNetCore.Mvc;
using Tonttutrack.BLL.Contracts;
using Tonttutrack.BLL.DTO;
using Tonttutrack.PL.Models;

namespace Tonttutrack.PL.Controllers;

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

    public IActionResult TrackerDevice()
    {
        return View("Views/Map/_TrackerDevice.cshtml");
    }

    [HttpPost("connectDevice")]
    public async Task<IActionResult> ConnectDeviceAsync([FromBody] DeviceConnectionViewModel userInput)
    {
        bool areCredentialsValid = await _deviceService.VerifyDeviceCredentialsAsync(userInput.Code, userInput.Password);

        if (!areCredentialsValid)
        {
            return BadRequest(new { message = "Invalid device code or password." });
        }

        bool deviceIsConnected = await _deviceCommunicationService.AuthorizeDeviceConnectionAsync(userInput.Code);
        deviceIsConnected |= await _deviceCommunicationService.AuthorizeDeviceConnectionAsync(userInput.Code);

        if (!deviceIsConnected)
        {
            return BadRequest(new {message = "Problem occurred while connecting to your device." });
        }

        return Json(true);
    }

    [HttpGet("readRoutePoint")]
    public async Task<IActionResult> ReadFromDeviceAsync()
    {
        RoutePointDTO routePoint = await _deviceCommunicationService.GetRoutePointDataAsync();

        if (!(routePoint.CurrentSpeed.ToString()).Contains('.'))
        {
            return BadRequest(new { message = "Problem occurred while connecting to your device." });
        }

        return Json(new {routePoint.Latitude, routePoint.Longitude, routePoint.CurrentSpeed });
    }
}