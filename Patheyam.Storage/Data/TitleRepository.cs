
namespace Patheyam.Storage.Data
{
    using Dapper;
    using Patheyam.Common;
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    public class TitleRepository : ITitleRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public TitleRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<TitleListDomain> GetTitleListAsync(SearchContract searchContract)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.GetTitles;
            var result = await connection.QueryMultipleAsync(procName, searchContract, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            var titles = await result.ReadAsync<TitleDomain>().ConfigureAwait(false);
            var pagination = await result.ReadAsync<PaginationInfo>().ConfigureAwait(false);
            return new TitleListDomain
            {
                Titles = titles.ToList(),
                Pagination = pagination.FirstOrDefault()
            };
        }
    }
}
