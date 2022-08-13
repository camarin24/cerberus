using Cerberus.Infrastructure.Utils;
using Cerberus.Infrastructure.Utils.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace Cerberus.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service)
    {
        service.AddScoped<IFileManager, FileManager>();
        return service;
    }
}