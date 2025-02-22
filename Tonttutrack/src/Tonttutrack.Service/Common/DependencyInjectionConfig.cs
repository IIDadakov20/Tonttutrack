using Microsoft.Extensions.DependencyInjection;
using Tonttutrack.Service.Contracts;
using Tonttutrack.Service.Services;

namespace Tonttutrack.Service.Common;

public static class DependencyInjectionConfig
{
    public static void AddServices(this IServiceCollection servicesCollection)
    {
        servicesCollection
            .AddSingleton<IDeviceCommunicationService, DeviceCommunicationService>()
            .AddScoped<IDeviceService, DeviceService>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<IRouteService, RouteService>();
    }
}