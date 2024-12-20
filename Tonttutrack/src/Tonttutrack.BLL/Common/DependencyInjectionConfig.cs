using Microsoft.Extensions.DependencyInjection;
using Tonttutrack.BLL.Contracts;
using Tonttutrack.BLL.Services;

namespace Tonttutrack.BLL.Common;

public static class DependencyInjectionConfig
{
    public static void AddServices(this IServiceCollection servicesCollection)
    {
        servicesCollection
            .AddSingleton<IDeviceCommunicationService, DeviceCommunicationService>()
            .AddScoped<IDeviceService, DeviceService>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IUserService, UserService>();
    }
}