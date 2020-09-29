namespace Patheyam.Domain.Models
{
    using System.Collections.Generic;
    public class SuccessFailureDomain
    {
        public List<int> SuccessIds { get; set; }
        public List<int> FailureIds { get; set; }
    }
}
