using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionAppSample
{
    public class Location : TableEntity
    {
        public Location()
        {
        }

        public Location(string countryCode, string name)
        {
            PartitionKey = countryCode;
            RowKey = name;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
