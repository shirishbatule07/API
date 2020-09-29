
namespace Patheyam.Domain.Interfaces
{
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICurrencyRepository
    {
        Task<CurrencyListDomain> GetCurrencyListAsync(SearchContract searchContract);
        Task<CurrencyDomain> GetCurrencyAsync(int currencyId);
        Task<int> AddOrUpdateCurrencyAsync(CurrencyContract currency, int userId);
        Task<bool> DeleteCurrencyAsync(int currencyId, int userId);
        Task<SuccessFailureDomain> DeleteCurrenciesByIdsAsync(List<int> currencyIds, int userId);
        Task<bool> UpdateCurrenciesStatusByIdsAsync(List<int> currencyIds, int userId, bool status);
    }
}
