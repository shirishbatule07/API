
namespace Patheyam.Contract.Models
{
    using Patheyam.Common;

    public class CurrencyContract
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
