
namespace Patheyam.Domain.Interfaces
{
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITimeZoneRepository
    {
        Task<TimeZoneListDomain> GetTimeZoneListAsync(SearchContract searchContract);
        Task<int> AddOrUpdateTimeZoneAsync(TimeZoneContract timeZone, int userId);
        Task<bool> DeleteTimeZoneAsync(int timeZoneId, int userId);
        Task<SuccessFailureDomain> DeleteTimeZonesByIdsAsync(List<int> timeZoneIds, int userId);
        Task<bool> UpdateTimeZonesStatusByIdsAsync(List<int> timeZoneIds, int userId, bool status);
    }
}
