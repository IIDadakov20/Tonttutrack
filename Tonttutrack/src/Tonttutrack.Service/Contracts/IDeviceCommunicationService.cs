using Tonttutrack.Domain.DTOs.Request;
using Tonttutrack.Domain.DTOs.Response;

namespace Tonttutrack.Service.Contracts;

public interface IDeviceCommunicationService
{
    Task<ErrorDTO> ConnectToBrokerAsync(DeviceRequestDTO deviceInfo);

    Task<bool> DisconnectFromBrokerAsync(string deviceCode);

    RoutePointDTO? GetRoutePointData(string deviceCode);
}