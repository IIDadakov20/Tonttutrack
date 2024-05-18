using System.IO.Ports;

namespace Tonttutrack.BLL.Contracts;

public interface IDeviceCommunicationService
{
    Task<bool> AuthorizeDeviceConnectionAsync();

    Task<string> ReceiveDeviceDataAsync(SerialPort port);

    Task<List<string>> GetRoutePointDataAsync();
}