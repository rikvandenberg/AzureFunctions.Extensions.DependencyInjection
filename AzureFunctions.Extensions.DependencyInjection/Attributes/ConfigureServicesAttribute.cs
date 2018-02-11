using System;
using Microsoft.Extensions.DependencyInjection;

namespace AzureFunctions.Extensions.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigureServicesAttribute : Attribute
    {
        public Type Config { get; }

        public ConfigureServicesAttribute(Type config)
        {
            Config = config;
        }
    }
}
