using System;
using System.Linq;
using Cerberus.Domain.Ports.Repository;
using Cerberus.Domain.Services;
using Cerberus.Domain.Utilities;
using Cerberus.Repository.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Cerberus.Api.Extensions;

public static class ApiExtensions
{
    public static IServiceCollection AddAppStore(this IServiceCollection service)
    {
        service.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        service.AddSingleton(typeof(IMessagesManager), typeof(MessageManager));

        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName != null && !a.FullName.StartsWith("Microsoft.VisualStudio.TraceDataCollector"));

        var services = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.CustomAttributes.Any(c => c.AttributeType == typeof(DomainService)));

        var repositories = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => !t.Name.StartsWith("I") && !t.Name.Contains("GenericRepository") &&
                        t.Name.Contains("Repository"));

        foreach (var re in repositories)
        {
            var repositoryInterface = re.GetInterfaces().FirstOrDefault(i => i.Name.Contains(re.Name));
            if (null == repositoryInterface) continue;
            service.AddTransient(repositoryInterface, re);
        }

        foreach (var se in services)
        {
            var serviceInterface = se.GetInterfaces().FirstOrDefault(i => i.FullName != null && i.FullName.Contains(se.Name));
            if (serviceInterface != null) service.AddTransient(serviceInterface, se);
        }

        return service;
    }
}