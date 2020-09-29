
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using Patheyam.Engine.Utils;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class DeleteCurrenciesByIdsCommand : ICommand<SuccessFailureDomain>
    {
        public List<int> Ids { get; set; }
        public int UserId { get; set; }
    }

    [AuditLog]
    public sealed class DeleteCurrenciesByIdsCommandHandler : ICommandHandler<DeleteCurrenciesByIdsCommand, SuccessFailureDomain>
    {
        private readonly ICurrencyRepository _crrencyRepository;
        public DeleteCurrenciesByIdsCommandHandler(ICurrencyRepository crrencyRepository)
        {
            _crrencyRepository = crrencyRepository;
        }

        public async Task<SuccessFailureDomain> Handle(DeleteCurrenciesByIdsCommand command)
        {
            command.Ids.ThrowIfNullOrEmpty<int>("Empty list parameter", nameof(command.Ids));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _crrencyRepository.DeleteCurrenciesByIdsAsync(command.Ids, command.UserId).ConfigureAwait(false);
        }
    }
}
