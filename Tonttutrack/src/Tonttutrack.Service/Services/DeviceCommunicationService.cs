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
using MQTTnet.Server;

namespace Tonttutrack.Service.Services;

internal class DeviceCommunicationService : IDeviceCommunicationService
{
    private readonly ConcurrentDictionary<string, (IMqttClient Client, ConcurrentDictionary<string, string> RoutePoints)> _mqttClients = new();
    private const string _mqttBrokerAddress = "";
    private const int _mqttBrokerPort = 1883;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeviceCommunicationService(
        IHttpContextAccessor httpContextAccessor,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ErrorDTO> ConnectToBrokerAsync(DeviceConnectionRequestDTO deviceInfo)
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

            string clientId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            if (!_mqttClients.ContainsKey(clientId))
            {
                var factory = new MqttFactory();
                var mqttClient = factory.CreateMqttClient();

                var _options = new MqttClientOptionsBuilder()
                    .WithTcpServer(_mqttBrokerAddress, _mqttBrokerPort)
                    .WithClientId(clientId.ToString())
                    .WithCleanSession()
                    .Build();

                try
                {
                    var connectResult = await mqttClient.ConnectAsync(_options);

                    if (connectResult.ResultCode != MqttClientConnectResultCode.Success)
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    result.Succeeded = false;
                    result.ErrorMessage.Add("", "Connection failed.");
                    return result;
                }

                _mqttClients.TryAdd(clientId, (mqttClient, new ConcurrentDictionary<string, string>()));
            }

            var client = _mqttClients[clientId].Client;

            await client.SubscribeAsync("statistics/" + deviceInfo.Code + "/speed");
            await client.SubscribeAsync("statistics/" + deviceInfo.Code + "/latitude");
            await client.SubscribeAsync("statistics/" + deviceInfo.Code + "/longitude");

            client.ApplicationMessageReceivedAsync -= async m => await OnMessageReceived(clientId, m);
            client.ApplicationMessageReceivedAsync += async m => await OnMessageReceived(clientId, m);

            string? deviceName = await deviceService.FetchConnectedDeviceNameAsync(deviceInfo.Code);

            result.Succeeded = true;
            result.ErrorMessage.Add("", $"{deviceName}");
            return result;
        }
    }

    private Task OnMessageReceived(string userId, MqttApplicationMessageReceivedEventArgs m)
    {
        if (_mqttClients.TryGetValue(userId, out var clientData))
        {
            string receivedTopic = m.ApplicationMessage.Topic.Substring(m.ApplicationMessage.Topic.IndexOf('/') + 1);
            string message = Encoding.UTF8.GetString(m.ApplicationMessage.PayloadSegment);
            clientData.RoutePoints.TryAdd(receivedTopic, message);
        }
        return Task.CompletedTask;
    }

    public async Task<bool> DisconnectFromBrokerAsync(string deviceCode)
    {
        string clientId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        if (_mqttClients.TryGetValue(clientId, out var clientData))
        {
            var client = clientData.Client;
            if (client.IsConnected)
            {
                await client.UnsubscribeAsync($"statistics/{deviceCode}/speed");
                await client.UnsubscribeAsync($"statistics/{deviceCode}/latitude");
                await client.UnsubscribeAsync($"statistics/{deviceCode}/longitude");

                await client.DisconnectAsync();
            }

            _mqttClients.TryRemove(clientId, out _);
            return true;
        }

        return false;
    }

    public RoutePointDTO? GetRoutePointData(string deviceCode)
    {
        var clientId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        _mqttClients.TryGetValue(clientId, out var clientData);
        var routePoints = clientData.RoutePoints;
        
        if (routePoints == null)
        {
            return null;
        }

        var topics = routePoints.Keys.Where(k => k.Contains(deviceCode)).ToList();

        routePoints.TryGetValue(topics.FirstOrDefault(k => k.Contains("speed"))!, out var speed);
        routePoints.TryGetValue(topics.FirstOrDefault(k => k.Contains("latitude"))!, out var latitude);
        routePoints.TryGetValue(topics.FirstOrDefault(k => k.Contains("longitude"))!, out var longitude);

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
                routePoints.TryRemove(topic, out _);
            }

            return result;
        }

        return null;
    }
}