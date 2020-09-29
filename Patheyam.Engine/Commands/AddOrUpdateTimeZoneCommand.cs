
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;

    
    public sealed class AddOrUpdateTimeZoneCommand : ICommand<int>
    {
        public TimeZoneContract TimeZone { get; set; }
        public int UserId { get; set; }
    }

    [AuditLog]
    public sealed class AddOrUpdateTimeZoneCommandHandler : ICommandHandler<AddOrUpdateTimeZoneCommand, int>
    {
        private readonly ITimeZoneRepository _timeZoneRepository;
        public AddOrUpdateTimeZoneCommandHandler(ITimeZoneRepository timeZoneRepository)
        {
            _timeZoneRepository = timeZoneRepository;
        }

        public async Task<int> Handle(AddOrUpdateTimeZoneCommand command)
        {
            command.TimeZone.Name.ThrowIfNullOrEmpty("Invalid time zone name parameter", nameof(command.TimeZone.Name));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _timeZoneRepository.AddOrUpdateTimeZoneAsync(command.TimeZone, command.UserId).ConfigureAwait(false);
        }
    }
}
