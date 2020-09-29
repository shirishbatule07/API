
namespace Patheyam.Storage.Data
{
    using Dapper;
    using Patheyam.Common;
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public CurrencyRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> AddOrUpdateCurrencyAsync(CurrencyContract currency, int userId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.UpsertCurrency;
            var result = await connection.QueryAsync<int>(procName, new
            {
                currency.Id,
                currency.Code,
                currency.Name,
                currency.SLName,
                currency.NumberOfDigits,
                currency.CurrencySymbol,
                currency.ExchangeRate,
                currency.SymbolPosition,
                currency.Active,
                userId
            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public async Task<SuccessFailureDomain> DeleteCurrenciesByIdsAsync(List<int> currencyIds, int userId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.DeleteCurrenciesByIds;
            var result = await connection.QueryMultipleAsync(procName, new
            {
                Ids = currencyIds.ConvertToDataTable("Value"),
                UserId = userId
            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            var failedIds = await result.ReadAsync<int>().ConfigureAwait(false);
            return new SuccessFailureDomain
            {
                SuccessIds = currencyIds.Where(ids => !failedIds.Contains(ids)).ToList(),
                FailureIds = failedIds.ToList()
            };
        }

        public async Task<bool> DeleteCurrencyAsync(int currencyId, int userId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.DeleteCurrency;
            await connection.QueryAsync(procName, new
            {
                Id = currencyId,
                UserId = userId
            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return true;
        }

        public async Task<CurrencyDomain> GetCurrencyAsync(int currencyId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.GetCurrencyById;
            var result = await connection.QuerySingleAsync<CurrencyDomain>(procName, new { Id = currencyId }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return result;
        }

        public async Task<CurrencyListDomain> GetCurrencyListAsync(SearchContract searchContract)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.GetCurrencies;
            var result = await connection.QueryMultipleAsync(procName, searchContract, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            var currencies = await result.ReadAsync<CurrencyDomain>().ConfigureAwait(false);
            var pagination = await result.ReadAsync<PaginationInfo>().ConfigureAwait(false);
            return new CurrencyListDomain
            {
                Currencies = currencies.ToList(),
                Pagination = pagination.FirstOrDefault()
            };
        }

        public async Task<bool> UpdateCurrenciesStatusByIdsAsync(List<int> currencyIds, int userId, bool status)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.UpdateCurrenciesStatusByIds;
            await connection.QueryAsync(procName, new { Ids = currencyIds.ConvertToDataTable("Value"), Status = status, UserId = userId }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return true;
        }
    }
}
