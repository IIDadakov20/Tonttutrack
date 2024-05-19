using System.IO.Ports;
using Tonttutrack.BLL.Contracts;

namespace Tonttutrack.BLL.Services;

internal class DeviceCommunicationService : IDeviceCommunicationService
{
    private SerialPort? _port;

    public async Task<bool> AuthorizeDeviceConnectionAsync(string authorizationCode)
    {
        foreach (var portName in SerialPort.GetPortNames())
        {
            SerialPort port = new(portName, 115200);
            port.Open();
            port.Write("Authentication code required");

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
        while (port.BytesToRead == 0)
        {
            await Task.Delay(1);
        }

        string receivedData = await Task.Run(() => port.ReadLine());
        return receivedData.TrimEnd('\r');
    }

    public async Task<List<string>> GetRoutePointDataAsync()
    {
        List<string> routePoint = new();
        int messageCount = 0;

        if (_port != null)
        {
            while (messageCount < 3)
            {
                string routePointParameter = await ReceiveDeviceDataAsync(_port);
                routePoint.Add(routePointParameter);
                messageCount++;
            }

            _port.DiscardInBuffer();
        }

        return routePoint;
    }
}