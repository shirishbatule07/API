
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;


    public sealed class AddOrUpdateCompanyCommand : ICommand<int>
    {
        public CompanyContract Company { get; set; }
        public int UserId { get; set; }
    }

    [AuditLog]
    public sealed class AddOrUpdateCompanyCommandHandler : ICommandHandler<AddOrUpdateCompanyCommand, int>
    {
        private readonly ICompanyRepository _companyRepository;
        public AddOrUpdateCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<int> Handle(AddOrUpdateCompanyCommand command)
        {
            
            command.Company.CompanyName.ThrowIfNullOrEmpty("Invalid company name parameter", nameof(command.Company.CompanyName));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _companyRepository.AddOrUpdateCompanyAsync(command.Company, command.UserId).ConfigureAwait(false);
        }
    }
}
