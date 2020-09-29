
namespace Patheyam.Engine.Queries
{
    using Patheyam.Common;
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;

    public sealed class GetStateListQuery : IQuery<StateListDomain>
    {
        public SearchContract SearchContract { get; set; }
    }

    [AuditLog]
    public sealed class GetStateListQueryHandler : IQueryHandler<GetStateListQuery, StateListDomain>
    {
        private readonly ILocationRepository _commonRepository;
        public GetStateListQueryHandler(ILocationRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public async Task<StateListDomain> Handle(GetStateListQuery query)
        {
            query.ThrowIfNull("Invalid query parameter", nameof(query));
            var searchContract = query.SearchContract;
            searchContract.PageNumber.ThrowIfNotPositiveNonZeroInt("Invalid page number parameter", nameof(searchContract.PageNumber));
            searchContract.PageSize.ThrowIfNotPositiveNonZeroInt("Invalid page size parameter", nameof(searchContract.PageSize));
            return await _commonRepository.GetStateListAsync(query.SearchContract).ConfigureAwait(false);
        }
    }
}
