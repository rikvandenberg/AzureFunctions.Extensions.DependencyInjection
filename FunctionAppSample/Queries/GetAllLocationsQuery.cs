using System.Collections.Generic;
using Vanguard.Framework.Core.Cqrs;

namespace FunctionAppSample
{
    public class GetAllLocationsQuery : IAsyncQuery<IEnumerable<Location>>
    {
        public GetAllLocationsQuery(string countryCode)
        {
            CountryCode = countryCode;
        }

        public string CountryCode { get; }
    }
}
