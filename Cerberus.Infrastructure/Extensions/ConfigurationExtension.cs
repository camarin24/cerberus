using System;
using Microsoft.Extensions.Configuration;

namespace Cerberus.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static string GetSectionValue(this IConfiguration configuration, string key)
    {
        return configuration.GetSection(key).Value;
    }

    public static int GetSectionVale(this IConfiguration configuration, string key)
    {
        return Convert.ToInt32(configuration.GetSection(key).Value);
    }
}