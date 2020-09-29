
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using Patheyam.Engine.Utils;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class DeleteTimeZonesByIdsCommand : ICommand<SuccessFailureDomain>
    {
        public List<int> Ids { get; set; }
        public int UserId { get; set; }
    }

    [AuditLog]
    public sealed class DeleteTimeZonesByIdsCommandHandler : ICommandHandler<DeleteTimeZonesByIdsCommand, SuccessFailureDomain>
    {
        private readonly ITimeZoneRepository _TimeZoneRepository;
        public DeleteTimeZonesByIdsCommandHandler(ITimeZoneRepository TimeZoneRepository)
        {
            _TimeZoneRepository = TimeZoneRepository;
        }

        public async Task<SuccessFailureDomain> Handle(DeleteTimeZonesByIdsCommand command)
        {
            command.Ids.ThrowIfNullOrEmpty<int>("Empty list parameter", nameof(command.Ids));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _TimeZoneRepository.DeleteTimeZonesByIdsAsync(command.Ids, command.UserId).ConfigureAwait(false);
        }
    }
}
