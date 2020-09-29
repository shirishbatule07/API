
namespace Patheyam.Domain.Models
{
    using System.Collections.Generic;
    public class CountryListDomain
    {
        public List<CountryDomain> Countries { get; set; }
        public PaginationInfo Pagination { get; set; }
    }
    public class CountryDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
