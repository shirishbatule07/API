
namespace Patheyam.Domain.Models
{
    using System.Collections.Generic;
    public class TitleListDomain
    {
        public List<TitleDomain> Titles { get; set; }
        public PaginationInfo Pagination { get; set; }
    }
    public class TitleDomain
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abbreviation { get; set; }
    }
}
