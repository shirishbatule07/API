
namespace Patheyam.Domain.Models
{
    using Patheyam.Contract.Models;
    using System.Collections.Generic;
    public class CityListDomain
    {
        public List<CityDomain> Cities { get; set; }
        public PaginationInfo Pagination { get; set; }
    }

    public class CityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
