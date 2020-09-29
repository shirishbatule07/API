
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

    public class CompanyRepository : ICompanyRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public CompanyRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> AddOrUpdateCompanyAsync(CompanyContract company, int userId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.UpsertCompany;
            var result = await connection.QueryAsync<int>(procName, new
            {
                company.Id,                                              
                company.CompanyName,
                company.contactNumber,
                company.Email,
                company.alternatenumber,
                company.address,
                company.GST,
                company.city,
                company.pincode,
                company.CompanyLogo,
                company.codetailinfo,
                company.operationatimeworkingday,
                company.yearofestablish,
                company.modeofpayment,
                company.wesiteurl,
                company.googlemap,
                userId
            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public async Task<SuccessFailureDomain> DeleteCompaniesByIdsAsync(List<int> companyIds, int userId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.DeleteCompaniesByIds;
            var result = await connection.QueryMultipleAsync(procName, new
            {
                Ids = companyIds.ConvertToDataTable("Value"),
                UserId = userId
            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            var failedIds = await result.ReadAsync<int>().ConfigureAwait(false);
            return new SuccessFailureDomain
            {
                SuccessIds = companyIds.Where(ids => !failedIds.Contains(ids)).ToList(),
                FailureIds = failedIds.ToList()
            };
        }

        public async Task<bool> DeleteCompanyAsync(int companyId, int userId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.DeleteCompany;
            await connection.QueryAsync(procName, new
            {
                Id = companyId,
                UserId = userId
            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return true;
        }

        public async Task<CompanyDomain> GetCompanyAsync(int companyId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.GetCompanyById;
            var result = await connection.QuerySingleAsync<CompanyDomain>(procName, new { Id = companyId }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return result;
        }

        public async Task<CompanyListDomain> GetCompanyListAsync(SearchContract searchContract)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.GetCompanies;
            var result = await connection.QueryMultipleAsync(procName, searchContract, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            var currencies = await result.ReadAsync<CompanyDomain>().ConfigureAwait(false);
            var pagination = await result.ReadAsync<PaginationInfo>().ConfigureAwait(false);
            return new CompanyListDomain
            {
                Companies = currencies.ToList(),
                Pagination = pagination.FirstOrDefault()
            };
        }

        public async Task<bool> UpdateCompaniesStatusByIdsAsync(List<int> companyIds, int userId, bool status)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.UpdateCompaniesStatusByIds;
            await connection.QueryAsync(procName, new { Ids = companyIds.ConvertToDataTable("Value"), Status = status, UserId = userId }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return true;
        }
    }
}
