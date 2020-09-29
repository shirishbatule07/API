
namespace Patheyam.Storage.Data
{
    using Dapper;
    using Patheyam.Common;
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    public class LocationRepository : ILocationRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public LocationRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<CityListDomain> GetCityListAsync(SearchContract searchContract)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.GetCities;
            var result = await connection.QueryMultipleAsync(procName,
                new
                {
                    searchContract.SearchTerm,
                    searchContract.PageNumber,
                    searchContract.PageSize
                }
                , null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            var cities = await result.ReadAsync<CityDomain>().ConfigureAwait(false);
            var pagination = await result.ReadAsync<PaginationInfo>().ConfigureAwait(false);
            return new CityListDomain
            {
                Cities = cities.ToList(),
                Pagination = pagination.FirstOrDefault()
            };
        }

        public async Task<StateListDomain> GetStateListAsync(SearchContract searchContract)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.GetStates;
            var result = await connection.QueryMultipleAsync(procName,
                new
                {
                    searchContract.SearchTerm,
                    searchContract.PageNumber,
                    searchContract.PageSize
                }
                , null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            var states = await result.ReadAsync<StateDomain>().ConfigureAwait(false);
            var pagination = await result.ReadAsync<PaginationInfo>().ConfigureAwait(false);
            return new StateListDomain
            {
                States = states.ToList(),
                Pagination = pagination.FirstOrDefault()
            };
        }


        public async Task<CountryListDomain> GetCountryListAsync(SearchContract searchContract)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.GetCountries;
            var result = await connection.QueryMultipleAsync(procName,
                new
                {
                    searchContract.SearchTerm,
                    searchContract.PageNumber,
                    searchContract.PageSize
                }
                , null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            var countries = await result.ReadAsync<CountryDomain>().ConfigureAwait(false);
            var pagination = await result.ReadAsync<PaginationInfo>().ConfigureAwait(false);
            return new CountryListDomain
            {
                Countries = countries.ToList(),
                Pagination = pagination.FirstOrDefault()
            };
        }

        public async Task<CityDomain> GetLocationByCityIdAsync(int cityId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.GetLocationByCityId;
            return await connection.QueryFirstOrDefaultAsync<CityDomain>(procName, new { cityId }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
        }

    }
}
