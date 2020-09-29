namespace Patheyam.Domain.Models
{
    using System;

    public class AuditDomain
    {
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastUpdatedById { get; set; }
        public string LastUpdatedByName { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
