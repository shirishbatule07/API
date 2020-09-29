
namespace Patheyam.Domain.Models
{
    using System.Collections.Generic;
    public class TimeZoneListDomain
    {
        public List<TimeZoneDomain> TimeZones { get; set; }
        public PaginationInfo Pagination { get; set; }
    }
    public class TimeZoneDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Hours { get; set; }
    }
}
