using System.Collections.Generic;
using AzureFunctions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Vanguard.Framework.Core.Cqrs;

namespace FunctionAppSample
{
    public class Startup
    {
        public Startup()
        {
            if(!DependencyInjection.IsInitialized)
            {
                DependencyInjection.Initialize(ConfigureServices);
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
            services.AddTransient<IAsyncQueryHandler<IEnumerable<Location>, GetAllLocationsQuery>, GetAllLocationsQueryHandler>();
            services.AddTransient<ILocationRepository, LocationTableStorageRepository>();
        }
    }
}
