
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;

    public sealed class DeleteCompanyCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }

    [AuditLog]
    public sealed class DeleteCompanyCommandHandler : ICommandHandler<DeleteCompanyCommand, bool>
    {
        private readonly ICompanyRepository _companyRepository;
        public DeleteCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<bool> Handle(DeleteCompanyCommand command)
        {
            command.Id.ThrowIfNotPositiveNonZeroInt("Invalid company id parameter", nameof(command.Id));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _companyRepository.DeleteCompanyAsync(command.Id, command.UserId).ConfigureAwait(false);
        }
    }
}
