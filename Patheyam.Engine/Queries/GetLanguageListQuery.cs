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

    public sealed class GetLanguageListQuery : IQuery<LanguageListDomain>
    {
        public SearchContract SearchContract { get; set; }
    }

    [AuditLog]
    public sealed class GetLanguageListQueryHandler : IQueryHandler<GetLanguageListQuery, LanguageListDomain>
    {
        private readonly ILanguageRepository _languageRepository;
        public GetLanguageListQueryHandler(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public async Task<LanguageListDomain> Handle(GetLanguageListQuery query)
        {
            query.ThrowIfNull("Invalid query parameter", nameof(query));
            var searchContract = query.SearchContract;
            searchContract.PageNumber.ThrowIfNotPositiveNonZeroInt("Invalid page number parameter", nameof(searchContract.PageNumber));
            searchContract.PageSize.ThrowIfNotPositiveNonZeroInt("Invalid page size parameter", nameof(searchContract.PageSize));
            return await _languageRepository.GetLanguageListAsync(query.SearchContract).ConfigureAwait(false);
        }
    }
}
