
namespace Patheyam.Engine.Queries
{
    using Patheyam.Common;
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;

    public sealed class GetTimeZoneListQuery : IQuery<TimeZoneListDomain>
    {
        public SearchContract SearchContract { get; set; }
    }

    [AuditLog]
    public sealed class GetBrandListQueryHandler : IQueryHandler<GetTimeZoneListQuery, TimeZoneListDomain>
    {
        private readonly ITimeZoneRepository _timeZoneRepository;
        public GetBrandListQueryHandler(ITimeZoneRepository timeZoneRepository)
        {
            _timeZoneRepository = timeZoneRepository;
        }

        public async Task<TimeZoneListDomain> Handle(GetTimeZoneListQuery query)
        {
            query.ThrowIfNull("Invalid query parameter", nameof(query));
            var searchContract = query.SearchContract;
            searchContract.PageNumber.ThrowIfNotPositiveNonZeroInt("Invalid page number parameter", nameof(searchContract.PageNumber));
            searchContract.PageSize.ThrowIfNotPositiveNonZeroInt("Invalid page size parameter", nameof(searchContract.PageSize));
            return await _timeZoneRepository.GetTimeZoneListAsync(query.SearchContract).ConfigureAwait(false);
        }
    }
}
