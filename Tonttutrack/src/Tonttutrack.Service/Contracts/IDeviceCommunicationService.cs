using System.IO.Ports;
using Tonttutrack.Service.DTO;

namespace Tonttutrack.Service.Contracts;

public interface IDeviceCommunicationService
{
    Task<bool> ConnectToBrokerAsync(string topic);

    RoutePointDTO? GetRoutePointData();

    //Task<bool> DisconnectDeviceAsync();
}