
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Engine.Utils;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class UpdateTimeZonesStatusByIdsCommand : ICommand<bool>
    {
        public List<int> Ids { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
    }

    [AuditLog]
    public sealed class UpdateTimeZonesStatusByIdsCommandHandler : ICommandHandler<UpdateTimeZonesStatusByIdsCommand, bool>
    {
        private readonly ITimeZoneRepository _timeZoneRepository;
        public UpdateTimeZonesStatusByIdsCommandHandler(ITimeZoneRepository timeZoneRepository)
        {
            _timeZoneRepository = timeZoneRepository;
        }

        public async Task<bool> Handle(UpdateTimeZonesStatusByIdsCommand command)
        {
            command.Ids.ThrowIfNullOrEmpty<int>("Empty list parameter", nameof(command.Ids));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _timeZoneRepository.UpdateTimeZonesStatusByIdsAsync(command.Ids, command.UserId, command.Status).ConfigureAwait(false);
        }
    }
}
