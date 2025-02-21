using Microsoft.AspNetCore.Http;
using MQTTnet.Client;
using MQTTnet;
using System.Security.Claims;
using Tonttutrack.Service.Contracts;
using Tonttutrack.Domain.DTOs.Response;
using System.Text;
using System.Collections.Concurrent;

namespace Tonttutrack.Service.Services;

internal class DeviceCommunicationService : IDeviceCommunicationService
{
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _options;
    private const string _mqttBrokerAddress = "192.168.0.102";
    private const int _mqttBrokerPort = 1883;
    private readonly ConcurrentDictionary<string, string> _routePoints = new();

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

        await _mqttClient.SubscribeAsync("statistics/" + deviceCode + "/speed");
        await _mqttClient.SubscribeAsync("statistics/" + deviceCode + "/latitude");
        await _mqttClient.SubscribeAsync("statistics/" + deviceCode + "/longitude");

        _mqttClient.ApplicationMessageReceivedAsync += m =>
        {
            string receivedTopic = m.ApplicationMessage.Topic.Substring(m.ApplicationMessage.Topic.IndexOf('/') + 1);
            string message = Encoding.UTF8.GetString(m.ApplicationMessage.PayloadSegment);
            _routePoints.TryAdd(receivedTopic, message);
            return Task.CompletedTask;
        };

        return true;
    }

    public RoutePointDTO? GetRoutePointData(string deviceCode)
    {
        var topics = new List<string>
        {
            $"{deviceCode}/speed",
            $"{deviceCode}/latitude",
            $"{deviceCode}/longitude"
        };

        if (_routePoints[topics[1]] != "0" &&
            _routePoints[topics[2]] != "0")
        {
            var result = new RoutePointDTO
            {
                Latitude = decimal.Parse(_routePoints[topics[1]]),
                Longitude = decimal.Parse(_routePoints[topics[2]]),
                CurrentSpeed = decimal.Parse(_routePoints[topics[0]])
            };

            foreach (var topic in topics)
            {
                _routePoints.TryRemove(topic, out _);
            }

            return result;
        }

        return null;
    }
}