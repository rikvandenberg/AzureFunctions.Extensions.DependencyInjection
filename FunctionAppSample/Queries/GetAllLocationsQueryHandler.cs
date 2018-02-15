using System.Collections.Generic;
using System.Threading.Tasks;
using Vanguard.Framework.Core.Cqrs;

namespace FunctionAppSample
{
    internal class GetAllLocationsQueryHandler : IAsyncQueryHandler<IEnumerable<Location>, GetAllLocationsQuery>
    {
        private readonly ILocationRepository _locationRepository;

        public GetAllLocationsQueryHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Location>> Retrieve(GetAllLocationsQuery query)
        {
            return await _locationRepository.GetAllAsync(query.CountryCode);
        }
    }
}
