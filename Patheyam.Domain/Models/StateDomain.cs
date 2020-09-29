
namespace Patheyam.Domain.Models
{

    using Patheyam.Contract.Models;
    using System.Collections.Generic;
    public class StateListDomain
    {
        public List<StateDomain> States { get; set; }
        public PaginationInfo Pagination { get; set; }
    }
    public class StateDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
