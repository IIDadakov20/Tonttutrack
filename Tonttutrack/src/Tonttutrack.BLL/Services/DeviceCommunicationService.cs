using System.IO.Ports;
using Tonttutrack.BLL.Contracts;
using Tonttutrack.BLL.DTO;

namespace Tonttutrack.BLL.Services;

internal class DeviceCommunicationService : IDeviceCommunicationService
{
    private static SerialPort? _port;

    private static async Task<string> ReceiveDeviceDataAsync(SerialPort port)
    {
        if (port.BytesToRead == 0)
            return string.Empty;

        string receivedData = await Task.Run(() => port.ReadLine());
        return receivedData.TrimEnd('\r');
    }

    public async Task<bool> AuthorizeDeviceConnectionAsync(string authorizationCode)
    {
        foreach (var portName in SerialPort.GetPortNames())
        {
            SerialPort port = new(portName, 115200);
            port.Open();
            port.Write("Authentication code required");
            await Task.Delay(1500);
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

    public async Task<RoutePointDTO> GetRoutePointDataAsync()
    {
        if (_port == null || !_port.IsOpen)
        {
            _port = null;
            return new RoutePointDTO { CurrentSpeed = -1 };
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

        if (string.IsNullOrEmpty(routePoint.FirstOrDefault()))
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

    public async Task<bool> DisconnectDeviceAsync()
    {
        if (_port == null || !_port.IsOpen)
        {
            return true;
        }

        _port.Write("break");
        string response = await ReceiveDeviceDataAsync(_port);

        while(response != "break")
        {
            await Task.Delay(100);
            response = await ReceiveDeviceDataAsync(_port);
        }

        if (response == "break")
        {
            _port.Close();
            return true;
        }

        return false;
    }
}