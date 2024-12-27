using Tonttutrack.Domain.DTOs.Response;

namespace Tonttutrack.Service.Contracts;

public interface IDeviceCommunicationService
{
    Task<bool> ConnectToBrokerAsync(string topic);

    RoutePointDTO? GetRoutePointData();

    //Task<bool> DisconnectDeviceAsync();
}