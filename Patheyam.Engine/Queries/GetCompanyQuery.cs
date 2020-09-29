
namespace Patheyam.Engine.Queries
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;

    public sealed class GetCompanyQuery : IQuery<CompanyDomain>
    {
        public int CompanyId { get; set; }
    }

    [AuditLog]
    public sealed class GetCompanyQueryHandler : IQueryHandler<GetCompanyQuery, CompanyDomain>
    {
        private readonly ICompanyRepository _companyRepository;
        public GetCompanyQueryHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyDomain> Handle(GetCompanyQuery query)
        {
            query.ThrowIfNull("Invalid query parameter", nameof(query));
            query.CompanyId.ThrowIfNotPositiveNonZeroInt("Invalid Company Id", nameof(query.CompanyId));
            return await _companyRepository.GetCompanyAsync(query.CompanyId).ConfigureAwait(false);
        }
    }
}
