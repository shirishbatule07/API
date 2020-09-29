
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Engine.Utils;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class UpdateCompaniesStatusByIdsCommand : ICommand<bool>
    {
        public List<int> Ids { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
    }

    [AuditLog]
    public sealed class UpdateCompaniesStatusByIdsCommandHandler : ICommandHandler<UpdateCompaniesStatusByIdsCommand, bool>
    {
        private readonly ICompanyRepository _companyRepository;
        public UpdateCompaniesStatusByIdsCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<bool> Handle(UpdateCompaniesStatusByIdsCommand command)
        {
            command.Ids.ThrowIfNullOrEmpty<int>("Empty list parameter", nameof(command.Ids));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _companyRepository.UpdateCompaniesStatusByIdsAsync(command.Ids, command.UserId, command.Status).ConfigureAwait(false);
        }
    }
}
