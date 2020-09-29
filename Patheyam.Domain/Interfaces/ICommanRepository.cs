
namespace Patheyam.Domain.Interfaces
{
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICommanRepository
    {
      
        Task<bool> UpdateStatusByIdsAsync(List<int> UpdateIds, int userId, bool status, string tablename);
        Task<SuccessFailureDomain> DeleteByIdsAsync(List<int> timeZoneIds, int userId);
    }
}
