using System;
using System.Collections.Generic;
using System.Text;

namespace Patheyam.Engine.Queries
{
    using Patheyam.Common;
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;
    public sealed class GetCountryListQuery : IQuery<CountryListDomain>
    {
        public SearchContract SearchContract { get; set; }
    }

    [AuditLog]
    public sealed class GetCountryListQueryHandler : IQueryHandler<GetCountryListQuery, CountryListDomain>
    {
        private readonly ILocationRepository _commonRepository;
        public GetCountryListQueryHandler(ILocationRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public async Task<CountryListDomain> Handle(GetCountryListQuery query)
        {
            query.ThrowIfNull("Invalid query parameter", nameof(query));
            var searchContract = query.SearchContract;
            searchContract.PageNumber.ThrowIfNotPositiveNonZeroInt("Invalid page number parameter", nameof(searchContract.PageNumber));
            searchContract.PageSize.ThrowIfNotPositiveNonZeroInt("Invalid page size parameter", nameof(searchContract.PageSize));
            return await _commonRepository.GetCountryListAsync(query.SearchContract).ConfigureAwait(false);
        }
    }
}
