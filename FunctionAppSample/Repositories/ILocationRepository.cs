using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionAppSample
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAllAsync(string countryCode);
    }
}
