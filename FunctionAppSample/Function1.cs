using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AzureFunctions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Vanguard.Framework.Core.Cqrs;

namespace FunctionAppSample
{
    [ConfigureServices(typeof(Startup))]
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/{countryCode}")] HttpRequestMessage req, 
            string countryCode,
            TraceWriter log,
            [Inject] IQueryDispatcher queryDispatcher)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var query = new GetAllLocationsQuery(countryCode);
            IEnumerable<Location> locations = await queryDispatcher.DispatchAsync(query);
            return req.CreateJsonResponse(HttpStatusCode.OK, locations.Select(l => new LocationDto { CountryCode = l.PartitionKey.ToUpper(), Name = l.RowKey, Latitude = l.Latitude, Longitude = l.Longitude }));
        }
    }
}
