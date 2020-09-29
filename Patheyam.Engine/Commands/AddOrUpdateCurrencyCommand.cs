
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;


    public sealed class AddOrUpdateCurrencyCommand : ICommand<int>
    {
        public CurrencyContract Currency { get; set; }
        public int UserId { get; set; }
    }

    [AuditLog]
    public sealed class AddOrUpdateCurrencyCommandHandler : ICommandHandler<AddOrUpdateCurrencyCommand, int>
    {
        private readonly ICurrencyRepository _currencyRepository;
        public AddOrUpdateCurrencyCommandHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<int> Handle(AddOrUpdateCurrencyCommand command)
        {
            command.Currency.Code.ThrowIfNullOrEmpty("Invalid currency code parameter", nameof(command.Currency.Code));
            command.Currency.Name.ThrowIfNullOrEmpty("Invalid currency name parameter", nameof(command.Currency.Name));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _currencyRepository.AddOrUpdateCurrencyAsync(command.Currency, command.UserId).ConfigureAwait(false);
        }
    }
}
