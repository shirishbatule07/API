
namespace Patheyam.Domain.Models
{
    using System.Collections.Generic;
    public class LanguageListDomain
    {
        public List<LanguageDomain> Languages { get; set; }
        public PaginationInfo Pagination { get; set; }
    }
    public class LanguageDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
