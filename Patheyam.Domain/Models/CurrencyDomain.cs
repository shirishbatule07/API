
namespace Patheyam.Domain.Models
{
    using Patheyam.Common;
    using System.Collections.Generic;
    public class CurrencyListDomain
    {
        public List<CurrencyDomain> Currencies { get; set; }
        public PaginationInfo Pagination { get; set; }
    }
    public class CurrencyDomain : AuditDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SLName { get; set; }
        public string Code { get; set; }
        public int NumberOfDigits { get; set; }
        public string CurrencySymbol { get; set; }
        public float ExchangeRate { get; set; }
        public SymbolPosition SymbolPosition { get; set; }
        public bool Active { get; set; }
    }
}
