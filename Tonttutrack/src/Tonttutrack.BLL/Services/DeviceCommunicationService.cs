using System.IO.Ports;
using Tonttutrack.BLL.Contracts;
using Tonttutrack.BLL.DTO;

namespace Tonttutrack.BLL.Services;

internal class DeviceCommunicationService : IDeviceCommunicationService
{
    private static SerialPort? _port;

    public async Task<bool> AuthorizeDeviceConnectionAsync(string authorizationCode)
    {
        foreach (var portName in SerialPort.GetPortNames())
        {
            SerialPort port = new(portName, 115200);
            port.Open();
            port.Write("Authentication code required");
            await Task.Delay(5000);
            string receivedCode = await ReceiveDeviceDataAsync(port);

            if (receivedCode == authorizationCode)
            {
                _port = port;
                return true;
            }
            else
            {
                port.Close();
            }
        }
        return false;
    }

    public async Task<string> ReceiveDeviceDataAsync(SerialPort port)
    {
        int retryCount = 0;
        while (port.BytesToRead == 0 && retryCount < 5)
        {
            await Task.Delay(1);
            retryCount++;
        }

        if (port.BytesToRead == 0)
            return string.Empty;

        string receivedData = await Task.Run(() => port.ReadLine());
        return receivedData.TrimEnd('\r');
    }

    public async Task<RoutePointDTO> GetRoutePointDataAsync()
    {
        if (_port == null || !_port.IsOpen)
        {
            return new RoutePointDTO();
        }

        List<string> routePoint = new();
        int messageCount = 0;

        while (messageCount < 3)
        {
            string routePointParameter = await ReceiveDeviceDataAsync(_port);
            routePoint.Add(routePointParameter);
            messageCount++;
        }

        _port.DiscardInBuffer();

        if (routePoint[0] == string.Empty)
        {
            return new RoutePointDTO();
        }

        return new RoutePointDTO
        {
            Latitude = decimal.Parse(routePoint[0]),
            Longitude = decimal.Parse(routePoint[1]),
            CurrentSpeed = decimal.Parse(routePoint[2])
        };
    }
}