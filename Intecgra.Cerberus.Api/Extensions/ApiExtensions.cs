using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Intecgra.Cerberus.Domain.Ports.Data;
using Intecgra.Cerberus.Domain.Services;
using Intecgra.Cerberus.Domain.Utilities;
using Intecgra.Cerberus.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Intecgra.Cerberus.Api.Extensions
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddAppStore(this IServiceCollection service)
        {
            service.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddSingleton(typeof(IMessagesManager), typeof(MessageManager));

            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.FullName.StartsWith("Microsoft.VisualStudio.TraceDataCollector"));

            var services = assemblies.SelectMany(a => a.GetTypes())
                .Where(t => t.CustomAttributes.Any(c => c.AttributeType == typeof(DomainService)));

            foreach (Type se in services)
            {
                var serviceInterface = se.GetInterfaces().FirstOrDefault(i => i.FullName.Contains(se.Name));
                service.AddTransient(serviceInterface, se);
            }
            return service;
        }
    }
}