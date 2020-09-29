
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Engine.Utils;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class UpdateStatusByIdsCommand : ICommand<bool>
    {
        public List<int> Ids { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
        public string Tablename { get; set; }
    }

    [AuditLog]
    public sealed class UpdateStatusByIdsCommandHandler : ICommandHandler<UpdateStatusByIdsCommand, bool>
    {
        private readonly ICommanRepository _updateRepository;
        public UpdateStatusByIdsCommandHandler(ICommanRepository updateRepository)
        {
            _updateRepository = updateRepository;
        }

        public async Task<bool> Handle(UpdateStatusByIdsCommand command)
        {
            command.Ids.ThrowIfNullOrEmpty<int>("Empty list parameter", nameof(command.Ids));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _updateRepository.UpdateStatusByIdsAsync(command.Ids, command.UserId, command.Status,command.Tablename).ConfigureAwait(false);
        }
    }
}
