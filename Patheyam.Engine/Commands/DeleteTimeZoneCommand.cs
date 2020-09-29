
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;


    public sealed class DeleteTimeZoneCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }

    [AuditLog]
    public sealed class DeleteTimeZoneCommandHandler : ICommandHandler<DeleteTimeZoneCommand, bool>
    {
        private readonly ITimeZoneRepository _timeZoneRepository;
        public DeleteTimeZoneCommandHandler(ITimeZoneRepository timeZoneRepository)
        {
            _timeZoneRepository = timeZoneRepository;
        }

        public async Task<bool> Handle(DeleteTimeZoneCommand command)
        {
            command.Id.ThrowIfNotPositiveNonZeroInt("Invalid time zone id parameter", nameof(command.Id));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _timeZoneRepository.DeleteTimeZoneAsync(command.Id, command.UserId).ConfigureAwait(false);
        }
    }
}
