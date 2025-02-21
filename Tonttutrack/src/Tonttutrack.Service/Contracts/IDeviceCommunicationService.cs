using Tonttutrack.Domain.DTOs.Response;

namespace Tonttutrack.Service.Contracts;

public interface IDeviceCommunicationService
{
    Task<bool> ConnectToBrokerAsync(string deviceCode);

    RoutePointDTO? GetRoutePointData(string deviceCode);

    //Task<bool> DisconnectDeviceAsync();
}