using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cerberus.Domain.Ports.Repository;
using Cerberus.Domain.Services;
using Cerberus.Domain.Utilities;
using Cerberus.Repository.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Cerberus.Api.Extensions
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

            var repositories = assemblies.SelectMany(a => a.GetTypes())
                .Where(t => !t.Name.StartsWith("I") && !t.Name.Contains("GenericRepository") &&
                            t.Name.Contains("Repository"));

            foreach (Type re in repositories)
            {
                var repositoryInterface = re.GetInterfaces().FirstOrDefault(i => i.Name.Contains(re.Name));
                if (null == repositoryInterface) continue;
                service.AddTransient(repositoryInterface, re);
            }

            foreach (Type se in services)
            {
                var serviceInterface = se.GetInterfaces().FirstOrDefault(i => i.FullName.Contains(se.Name));
                service.AddTransient(serviceInterface, se);
            }

            return service;
        }
    }
}