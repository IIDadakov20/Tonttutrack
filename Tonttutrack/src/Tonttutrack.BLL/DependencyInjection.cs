using Microsoft.Extensions.DependencyInjection;
using Tonttutrack.BLL.Contracts;
using Tonttutrack.BLL.Services;

namespace Tonttutrack.BLL;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection servicesCollection)
    {
        servicesCollection
            .AddScoped<IDeviceCommunicationService, DeviceCommunicationService>();
    }
}