using Tonttutrack.DAL.Data;
using Tonttutrack.BLL.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Tonttutrack.BLL.Services;

internal class DeviceService : IDeviceService
{
    private readonly TonttutrackDbContext _context;

    public DeviceService(TonttutrackDbContext context)
    {
        _context = context;
    }

    public async Task<bool> VerifyDeviceCredentialsAsync(string code, string password)
    {
        var device = await _context.Devices.FirstOrDefaultAsync(d => d.Code == code);

        if (device == null)
            return false;

        return device.Password == password;
    }

    public async Task<string?> FetchConnectedDeviceName(string code)
    {
        var device = await _context.Devices.FirstOrDefaultAsync(d => d.Code == code);

        if (device == null)
            return null;
        
        return device.Name;
    }
}