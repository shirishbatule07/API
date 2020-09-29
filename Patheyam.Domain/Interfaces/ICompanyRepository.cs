
namespace Patheyam.Domain.Interfaces
{
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICompanyRepository
    {
        Task<CompanyListDomain> GetCompanyListAsync(SearchContract searchContract);
        Task<CompanyDomain> GetCompanyAsync(int currencyId);
        Task<int> AddOrUpdateCompanyAsync(CompanyContract currency, int userId);
        Task<bool> DeleteCompanyAsync(int currencyId, int userId);
        Task<SuccessFailureDomain> DeleteCompaniesByIdsAsync(List<int> currencyIds, int userId);
        Task<bool> UpdateCompaniesStatusByIdsAsync(List<int> currencyIds, int userId, bool status);
    }
}
