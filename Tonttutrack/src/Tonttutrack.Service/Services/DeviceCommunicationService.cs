using Microsoft.AspNetCore.Http;
using MQTTnet.Client;
using MQTTnet;
using System.Security.Claims;
using Tonttutrack.Service.Contracts;
using Tonttutrack.Domain.DTOs.Response;
using System.Text;
using System.Collections.Concurrent;
using Tonttutrack.Domain.DTOs.Request;
using Microsoft.Extensions.DependencyInjection;

namespace Tonttutrack.Service.Services;

internal class DeviceCommunicationService : IDeviceCommunicationService
{
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _options;
    private const string _mqttBrokerAddress = "192.168.0.102";
    private const int _mqttBrokerPort = 1883;
    private readonly ConcurrentDictionary<string, string> _routePoints = new();
    private readonly IServiceProvider _serviceProvider;

    public DeviceCommunicationService(
        IHttpContextAccessor httpContextAccessor,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        string clientId = httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        var factory = new MqttFactory();
        _mqttClient = factory.CreateMqttClient();

        _options = new MqttClientOptionsBuilder()
            .WithTcpServer(_mqttBrokerAddress, _mqttBrokerPort)
            .WithClientId(clientId.ToString())
            .WithCleanSession()
            .Build();
    }

    public async Task<ErrorDTO> ConnectToBrokerAsync(DeviceRequestDTO deviceInfo)
    {
        var result = new ErrorDTO();

        using (var scope = _serviceProvider.CreateScope())
        {
            var deviceService = scope.ServiceProvider.GetRequiredService<IDeviceService>();

            bool areCredentialsValid = await deviceService.VerifyDeviceCredentialsAsync(deviceInfo.Code, deviceInfo.Password);

            if (!areCredentialsValid)
            {
                result.Succeeded = false;
                result.ErrorMessage.Add("", "Invalid device code or password.");
                return result;
            }

            if (!this._mqttClient.IsConnected)
            {
                var connectResult = await this._mqttClient.ConnectAsync(_options);

                if (connectResult.ResultCode != MqttClientConnectResultCode.Success)
                {
                    result.Succeeded = false;
                    result.ErrorMessage.Add("", "Problem occurred while connecting to your device.");
                    return result;
                }
            }

            await _mqttClient.SubscribeAsync("statistics/" + deviceInfo.Code + "/speed");
            await _mqttClient.SubscribeAsync("statistics/" + deviceInfo.Code + "/latitude");
            await _mqttClient.SubscribeAsync("statistics/" + deviceInfo.Code + "/longitude");

            _mqttClient.ApplicationMessageReceivedAsync -= OnMessageReceived;
            _mqttClient.ApplicationMessageReceivedAsync += OnMessageReceived;

            string? deviceName = await deviceService.FetchConnectedDeviceNameAsync(deviceInfo.Code);

            result.Succeeded = true;
            result.ErrorMessage.Add("", $"{deviceName}");
            return result;
        }
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