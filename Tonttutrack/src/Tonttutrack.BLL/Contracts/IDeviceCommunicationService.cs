using System.IO.Ports;
using Tonttutrack.BLL.DTO;

namespace Tonttutrack.BLL.Contracts;

public interface IDeviceCommunicationService
{
    Task<bool> AuthorizeDeviceConnectionAsync(string authorizationCode);

    Task<string> ReceiveDeviceDataAsync(SerialPort port);

    Task<RoutePointDTO> GetRoutePointDataAsync();
}