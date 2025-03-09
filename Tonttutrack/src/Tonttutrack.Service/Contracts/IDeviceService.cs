using Tonttutrack.Domain.DTOs.Request;
using Tonttutrack.Domain.DTOs.Response;

namespace Tonttutrack.Service.Contracts;

public interface IDeviceService
{
    Task<(List<DeviceResponseDTO>, int)> GetDevicesAsync(int pageNumber);

    Task<ErrorDTO> AddOrUpdateDeviceAsync(DeviceRequestDTO deviceInfo);

    Task<ErrorDTO> DeleteDeviceAsync(Guid deviceId);

    Task<bool> VerifyDeviceCredentialsAsync(string code, string password);

    Task<string?> FetchConnectedDeviceNameAsync(string code);
}