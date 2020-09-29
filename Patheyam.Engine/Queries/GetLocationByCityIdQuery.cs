
namespace Patheyam.Engine.Queries
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;

    public sealed class GetLocationByCityIdQuery : IQuery<CityDomain>
    {
        public int CityId { get; set; }
    }

    [AuditLog]
    public sealed class GetLocationByCityIdQueryHandler : IQueryHandler<GetLocationByCityIdQuery, CityDomain>
    {
        private readonly ILocationRepository _commonRepository;
        public GetLocationByCityIdQueryHandler(ILocationRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public async Task<CityDomain> Handle(GetLocationByCityIdQuery query)
        {
            query.ThrowIfNull("Invalid query parameter", nameof(query));
            query.CityId.ThrowIfNotPositiveNonZeroInt("Invalid city id parameter", nameof(query.CityId));
            return await _commonRepository.GetLocationByCityIdAsync(query.CityId).ConfigureAwait(false);
        }
    }
}
