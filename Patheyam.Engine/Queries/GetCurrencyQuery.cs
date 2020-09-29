
namespace Patheyam.Engine.Queries
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;

    public sealed class GetCurrencyQuery : IQuery<CurrencyDomain>
    {
        public int CurrencyId { get; set; }
    }

    [AuditLog]
    public sealed class GetCurrencyQueryHandler : IQueryHandler<GetCurrencyQuery, CurrencyDomain>
    {
        private readonly ICurrencyRepository _currencyRepository;
        public GetCurrencyQueryHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<CurrencyDomain> Handle(GetCurrencyQuery query)
        {
            query.ThrowIfNull("Invalid query parameter", nameof(query));
            query.CurrencyId.ThrowIfNotPositiveNonZeroInt("Invalid Currency Id", nameof(query.CurrencyId));
            return await _currencyRepository.GetCurrencyAsync(query.CurrencyId).ConfigureAwait(false);
        }
    }
}
