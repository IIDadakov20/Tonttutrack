using System.IO.Ports;
using Tonttutrack.BLL.DTO;

namespace Tonttutrack.BLL.Contracts;

public interface IDeviceCommunicationService
{
    Task<bool> ConnectToBrokerAsync(string topic);

    RoutePointDTO? GetRoutePointData();

    //Task<bool> DisconnectDeviceAsync();
}