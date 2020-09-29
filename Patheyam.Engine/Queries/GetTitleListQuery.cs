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
    public sealed class GetTitleListQuery : IQuery<TitleListDomain>
    {
        public SearchContract SearchContract { get; set; }
    }

    [AuditLog]
    public sealed class GetTitleListQueryHandler : IQueryHandler<GetTitleListQuery, TitleListDomain>
    {
        private readonly ITitleRepository _titleRepository;
        public GetTitleListQueryHandler(ITitleRepository titleRepository)
        {
            _titleRepository = titleRepository;
        }

        public async Task<TitleListDomain> Handle(GetTitleListQuery query)
        {
            query.ThrowIfNull("Invalid query parameter", nameof(query));
            var searchContract = query.SearchContract;
            searchContract.PageNumber.ThrowIfNotPositiveNonZeroInt("Invalid page number parameter", nameof(searchContract.PageNumber));
            searchContract.PageSize.ThrowIfNotPositiveNonZeroInt("Invalid page size parameter", nameof(searchContract.PageSize));
            return await _titleRepository.GetTitleListAsync(query.SearchContract).ConfigureAwait(false);
        }
    }
}
