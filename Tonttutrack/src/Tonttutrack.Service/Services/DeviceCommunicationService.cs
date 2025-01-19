using Microsoft.AspNetCore.Http;
using MQTTnet.Client;
using MQTTnet;
using System.Security.Claims;
using Tonttutrack.Service.Contracts;
using Tonttutrack.Domain.DTOs.Response;
using System.Text;

namespace Tonttutrack.Service.Services;

internal class DeviceCommunicationService : IDeviceCommunicationService
{
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _options;
    private const string _mqttBrokerAddress = "192.168.0.102";
    private const int _mqttBrokerPort = 1883;
    private readonly Dictionary<string, string> _routePoint = new();

    public DeviceCommunicationService(IHttpContextAccessor httpContextAccessor)
    {
        string clientId = httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        var factory = new MqttFactory();
        _mqttClient = factory.CreateMqttClient();

        _options = new MqttClientOptionsBuilder()
            .WithTcpServer(_mqttBrokerAddress, _mqttBrokerPort)
            .WithClientId(clientId.ToString())
            .WithCleanSession()
            .Build();
    }

    public async Task<bool> ConnectToBrokerAsync(string deviceCode)
    {
        if (!this._mqttClient.IsConnected)
        {
            var connectResult = await this._mqttClient.ConnectAsync(_options);

            if (connectResult.ResultCode != MqttClientConnectResultCode.Success)
                return false;
        }

        await _mqttClient.SubscribeAsync("car_statistics/" + deviceCode + "/speed");
        await _mqttClient.SubscribeAsync("car_statistics/" + deviceCode + "/latitude");
        await _mqttClient.SubscribeAsync("car_statistics/" + deviceCode + "/longitude");

        _mqttClient.ApplicationMessageReceivedAsync += m =>
        {
            string receivedTopic = m.ApplicationMessage.Topic.Substring(m.ApplicationMessage.Topic.LastIndexOf('/') + 1);
            string message = Encoding.UTF8.GetString(m.ApplicationMessage.PayloadSegment);
            _routePoint.Add(receivedTopic, message);
            return Task.CompletedTask;
        };

        return true;
    }

    public RoutePointDTO? GetRoutePointData()
    {
        if (_routePoint["latitude"] != "0" &&
            _routePoint["longitude"] != "0")
        {
            var result = new RoutePointDTO
            {
                Latitude = decimal.Parse(_routePoint["latitude"]),
                Longitude = decimal.Parse(_routePoint["longitude"]),
                CurrentSpeed = decimal.Parse(_routePoint["speed"])
            };

            _routePoint.Remove("latitude");
            _routePoint.Remove("longitude");
            _routePoint.Remove("speed");

            return result;
        }

        return null;
    }
}