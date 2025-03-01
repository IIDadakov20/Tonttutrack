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

        _mqttClient.ApplicationMessageReceivedAsync -= OnMessageReceived;
        _mqttClient.ApplicationMessageReceivedAsync += OnMessageReceived;

        return true;
    }

    private Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs m)
    {
        string receivedTopic = m.ApplicationMessage.Topic.Substring(m.ApplicationMessage.Topic.IndexOf('/') + 1);
        string message = Encoding.UTF8.GetString(m.ApplicationMessage.PayloadSegment);
        _routePoints.TryAdd(receivedTopic, message);
        return Task.CompletedTask;
    }

    public async Task<bool> DisconnectFromBrokerAsync(string deviceCode)
    {
        if (this._mqttClient.IsConnected)
        {
            await _mqttClient.UnsubscribeAsync("statistics/" + deviceCode + "/speed");
            await _mqttClient.UnsubscribeAsync("statistics/" + deviceCode + "/latitude");
            await _mqttClient.UnsubscribeAsync("statistics/" + deviceCode + "/longitude");

            await _mqttClient.DisconnectAsync();
        }

        var topics = _routePoints.Keys.Where(k => k.Contains(deviceCode)).ToList();
        foreach (var topic in topics)
        {
            _routePoints.TryRemove(topic, out _);
        }

        _mqttClient.ApplicationMessageReceivedAsync -= OnMessageReceived;

        return true;
    }

    public RoutePointDTO? GetRoutePointData(string deviceCode)
    {
        var topics = _routePoints.Keys.Where(k => k.Contains(deviceCode)).ToList();

        if (!topics.Any())
        {
            return null;
        }

        _routePoints.TryGetValue(topics.FirstOrDefault(k => k.Contains("speed"))!, out var speed);
        _routePoints.TryGetValue(topics.FirstOrDefault(k => k.Contains("latitude"))!, out var latitude);
        _routePoints.TryGetValue(topics.FirstOrDefault(k => k.Contains("longitude"))!, out var longitude);

        if (latitude != "0" && longitude != "0")
        {
            var result = new RoutePointDTO
            {
                Latitude = decimal.Parse(latitude!),
                Longitude = decimal.Parse(longitude!),
                CurrentSpeed = decimal.Parse(speed!)
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