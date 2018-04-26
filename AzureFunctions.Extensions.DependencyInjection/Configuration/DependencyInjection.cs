using Microsoft.Extensions.DependencyInjection;
using System;

namespace AzureFunctions.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        private static IServiceProvider _serviceProvider;

        public static bool IsInitialized { get; private set; } = false;

        public static void Initialize(Action<IServiceCollection> configureServices)
        {
            if (!IsInitialized)
            {
                var services = new ServiceCollection();
                configureServices(services);
                _serviceProvider = services.BuildServiceProvider();
                IsInitialized = true;
            }
        }

        public static object Resolve(Type type, string name)
        {
            if (!IsInitialized)
            {
                throw new InitializationException("DependencyInjection.Initialize must be called before dependencies can be resolved.");
            }
            return _serviceProvider.GetRequiredService(type);
        }
    }
}
