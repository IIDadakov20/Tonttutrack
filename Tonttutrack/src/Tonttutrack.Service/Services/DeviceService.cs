using Tonttutrack.DataAccess.Data;
using Tonttutrack.Service.Contracts;
using Microsoft.EntityFrameworkCore;
using Tonttutrack.Domain.DTOs.Response;
using Tonttutrack.Domain.DTOs.Request;
using Tonttutrack.DataAccess.Data.Models;
using Npgsql;

namespace Tonttutrack.Service.Services;

internal class DeviceService : IDeviceService
{
    private readonly TonttutrackDbContext _context;

    public DeviceService(TonttutrackDbContext context)
    {
        _context = context;
    }

    public async Task<bool> VerifyDeviceCredentialsAsync(string code, string password)
    {
        var deviceCode = new NpgsqlParameter("@device_code", System.Data.SqlDbType.Text);
        var devicePassword = new NpgsqlParameter("@password", System.Data.SqlDbType.Text);
        deviceCode.Value = code;
        devicePassword.Value = password;

        var device = await _context.Devices
            .FromSqlRaw("SELECT * FROM verify_device_password(@device_code, @password)", deviceCode, devicePassword)
            .FirstOrDefaultAsync();

        return device != null;
    }

    public async Task<string?> FetchConnectedDeviceNameAsync(string code)
    {
        var device = await _context.Devices.FirstOrDefaultAsync(d => d.Code == code);

        if (device == null)
            return null;
        
        return device.Name;
    }

    public async Task<(List<DeviceResponseDTO>, int)> GetDevicesAsync(int pageNumber)
    {
        var skip = (pageNumber - 1) * 4;

        var totalDevices = await _context.Devices.CountAsync();

        var devices = await _context.Devices
            .OrderBy(d => d.Id)
            .Skip(skip)
            .Take(4)
            .Select(d => new DeviceResponseDTO
            {
                Id = d.Id,
                Name = d.Name,
                Code = d.Code,
            })
            .ToListAsync()
            .ConfigureAwait(false);

        return (devices, totalDevices);
    }

    public async Task<ErrorDTO> AddOrUpdateDeviceAsync(DeviceRequestDTO deviceInfo)
    {
        var result = new ErrorDTO();

        var device = await _context.Devices
            .Where(d => d.Code == deviceInfo.Code)
            .FirstOrDefaultAsync();

        if (device == null)
        {
            Device newDevice = new Device
            {
                Code = deviceInfo.Code,
                Name = deviceInfo.Name,
                PasswordHash = deviceInfo.PasswordHash
            };

            await _context.Devices.AddAsync(newDevice);
        }
        else
        {
            device.Name = deviceInfo.Name;
            device.PasswordHash = deviceInfo.PasswordHash;
        }

        var deviceResult = await _context.SaveChangesAsync();

        if (deviceResult > 0)
        {
            result.Succeeded = true;
            return result;
        }

        result.Succeeded = false;
        result.ErrorMessage.Add("", "Problem occurred while saving the device");
        return result;
    }

    public async Task<ErrorDTO> DeleteDeviceAsync(Guid deviceId)
    {
        var result = new ErrorDTO();

        var device = await _context.Devices.FindAsync(deviceId);

        if (device == null)
        {
            result.Succeeded = true;
            return result;
        }

        var deviceDeletionResult = await _context.Devices
            .Where(d => d.Id == deviceId)
            .ExecuteDeleteAsync();

        if (deviceDeletionResult > 0)
        {
            result.Succeeded = true;
            return result;
        }

        result.Succeeded = false;
        result.ErrorMessage.Add("", "Problem occurred while deleting the device");
        return result;
    }
}