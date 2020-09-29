
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;

    public sealed class DeleteCurrencyCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }

    [AuditLog]
    public sealed class DeleteCurrencyCommandHandler : ICommandHandler<DeleteCurrencyCommand, bool>
    {
        private readonly ICurrencyRepository _currencyRepository;
        public DeleteCurrencyCommandHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<bool> Handle(DeleteCurrencyCommand command)
        {
            command.Id.ThrowIfNotPositiveNonZeroInt("Invalid currency id parameter", nameof(command.Id));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _currencyRepository.DeleteCurrencyAsync(command.Id, command.UserId).ConfigureAwait(false);
        }
    }
}
