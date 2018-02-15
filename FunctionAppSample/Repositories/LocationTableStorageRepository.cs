using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionAppSample
{
    internal class LocationTableStorageRepository : ILocationRepository
    {
        private readonly CloudTable _table;

        public LocationTableStorageRepository()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("AzureWebJobsStorage"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference("locations");
        }

        public async Task<IEnumerable<Location>> GetAllAsync(string countryCode)
        {
#if DEBUG
            // Test Data
            await _table.CreateIfNotExistsAsync();
            TableOperation rotterdam = TableOperation.InsertOrReplace(new Location("nl", "Rotterdam") { Latitude = 51.9244201, Longitude = 4.4777325 });
            await _table.ExecuteAsync(rotterdam);
#endif

            List<Location> locations = new List<Location>();
            TableQuery<Location> tableQuery = new TableQuery<Location>();
            TableContinuationToken continuationToken = null;

            do
            {
                // Retrieve a segment (up to 1,000 entities).
                TableQuerySegment<Location> page = await _table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
                continuationToken = page.ContinuationToken;

                locations.AddRange(page.Results);
                Console.WriteLine("Rows retrieved {0}", page.Results.Count);
            } while (continuationToken != null);

            return locations;
        }
    }
}
