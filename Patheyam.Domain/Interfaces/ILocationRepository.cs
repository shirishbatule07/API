
namespace Patheyam.Domain.Interfaces
{
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Models;
    using System.Threading.Tasks;

    public interface ILocationRepository
    {
        Task<CityListDomain> GetCityListAsync(SearchContract searchContract);
        Task<StateListDomain> GetStateListAsync(SearchContract searchContract);
        Task<CountryListDomain> GetCountryListAsync(SearchContract searchContract);
        Task<CityDomain> GetLocationByCityIdAsync(int cityId);
    }
}
