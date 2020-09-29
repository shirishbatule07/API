
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using Patheyam.Engine.Utils;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class DeleteCompaniesByIdsCommand : ICommand<SuccessFailureDomain>
    {
        public List<int> Ids { get; set; }
        public int UserId { get; set; }
    }

    [AuditLog]
    public sealed class DeleteCompaniesByIdsCommandHandler : ICommandHandler<DeleteCompaniesByIdsCommand, SuccessFailureDomain>
    {
        private readonly ICompanyRepository _companyRepository;
        public DeleteCompaniesByIdsCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<SuccessFailureDomain> Handle(DeleteCompaniesByIdsCommand command)
        {
            command.Ids.ThrowIfNullOrEmpty<int>("Empty list parameter", nameof(command.Ids));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _companyRepository.DeleteCompaniesByIdsAsync(command.Ids, command.UserId).ConfigureAwait(false);
        }
    }
}
