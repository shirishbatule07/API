
namespace Patheyam.Contract.Models
{
    public class SearchContract
    {
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool Active { get; set; }
    }
}
