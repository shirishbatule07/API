
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Engine.Utils;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class UpdateCurrenciesStatusByIdsCommand : ICommand<bool>
    {
        public List<int> Ids { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
    }

    [AuditLog]
    public sealed class UpdateCurrenciesStatusByIdsCommandHandler : ICommandHandler<UpdateCurrenciesStatusByIdsCommand, bool>
    {
        private readonly ICurrencyRepository _currencyRepository;
        public UpdateCurrenciesStatusByIdsCommandHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<bool> Handle(UpdateCurrenciesStatusByIdsCommand command)
        {
            command.Ids.ThrowIfNullOrEmpty<int>("Empty list parameter", nameof(command.Ids));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _currencyRepository.UpdateCurrenciesStatusByIdsAsync(command.Ids, command.UserId, command.Status).ConfigureAwait(false);
        }
    }
}
