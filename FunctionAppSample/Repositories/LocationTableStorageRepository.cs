using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
#if NETSTANDARD2_0
using Microsoft.WindowsAzure.Storage.Auth;
#endif
using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionAppSample
{
    internal class LocationTableStorageRepository : ILocationRepository
    {
        private readonly CloudTable _table;

        public LocationTableStorageRepository()
        {
            CloudStorageAccount storageAccount = null;
#if NET46 || NET461 || NET462
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("AzureWebJobsStorage"));
#endif
#if NETSTANDARD2_0
            var storageCredentials = new StorageCredentials("devstoreaccount1", "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==");
            storageAccount = new CloudStorageAccount(storageCredentials, 
                new Uri("http://127.0.0.1:10000/devstoreaccount1"),
                new Uri("http://127.0.0.1:10001/devstoreaccount1"),
                new Uri("http://127.0.0.1:10002/devstoreaccount1"),
                null
                );
#endif
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
