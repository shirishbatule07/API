

namespace Patheyam.Domain.Interfaces
{
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Models;
    using System.Threading.Tasks;
    public interface ILanguageRepository
    {
        Task<LanguageListDomain> GetLanguageListAsync(SearchContract searchContract);
    }
}
