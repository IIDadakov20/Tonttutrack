using System.IO.Ports;
using Tonttutrack.BLL.DTO;

namespace Tonttutrack.BLL.Contracts;

public interface IDeviceCommunicationService
{
    Task<bool> AuthorizeDeviceConnectionAsync(string authorizationCode);

    Task<RoutePointDTO> GetRoutePointDataAsync();

    Task<bool> DisconnectDeviceAsync();
}