using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tonttutrack.Domain.DTOs.Request;
using Tonttutrack.Service.Contracts;

namespace Tonttutrack.Web.Controllers;

[Authorize(Roles = "Admin")]
[Route("device")]
public class DeviceController : Controller
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpPost("createOrUpdateDevice")]
    public async Task<IActionResult> CreateDeviceAsync([FromBody]DeviceRequestDTO deviceInfo)
    {
        var deviceResult = await _deviceService.AddOrUpdateDeviceAsync(deviceInfo);

        if (!deviceResult.Succeeded)
        {
            return BadRequest(deviceResult.ErrorMessage.Values);
        }

        return Ok();
    }

    [HttpGet("getDevices")]
    public async Task<IActionResult> GetDevicesAsync([FromQuery] int pageNumber = 1)
    {
        var devices = await _deviceService.GetDevicesAsync(pageNumber);

        if (!devices.Item1.Any())
        {
            return BadRequest(new { message = "No devices found" });
        }

        return Ok(devices.Item1);
    }

    [HttpDelete("deleteDevice")]
    public async Task<IActionResult> DeleteDeviceAsync([FromBody] Guid deviceId)
    {
        var routeDeleteResult = await _deviceService.DeleteDeviceAsync(deviceId);

        if (!routeDeleteResult.Succeeded)
        {
            return BadRequest(routeDeleteResult.ErrorMessage.Values);
        }

        return Ok();
    }
}