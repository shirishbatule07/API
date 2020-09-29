
namespace Patheyam.Storage.Data
{
    using Dapper;
    using Patheyam.Common;
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    public class CommanRepository : ICommanRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public CommanRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool TryConnect(out string message)
        {
            using var connection = _connectionFactory.GetDbConnection();
            try
            {
                connection.Execute("SELECT TOP 1 1");
                message = null;
                return true;
            }
            catch (SqlException ex)
            {
                message = $"Failed to connect successfully to database '{connection.Database}'.\n{ex.GetType().Name}: {ex.Message}";
                return false;
            }
        }

        public async Task<SuccessFailureDomain> DeleteByIdsAsync(List<int> Ids, int userId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.DeletesByIds;
            var result = await connection.QueryMultipleAsync(procName, new
            {
                Ids = Ids.ConvertToDataTable("Value"),
                UserId = userId
            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            var failedIds = await result.ReadAsync<int>().ConfigureAwait(false);
            return new SuccessFailureDomain
            {
                SuccessIds = Ids.Where(ids => !failedIds.Contains(ids)).ToList(),
                FailureIds = failedIds.ToList()
            };
        }

        public async Task<bool> UpdateStatusByIdsAsync(List<int> Ids, int userId, bool status,string tablename)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.UpdateStatusByIds;
            await connection.QueryAsync(procName, new { 
                Ids = Ids.ConvertToDataTable("Value"), 
                Status = status,
                UserId = userId,
                @TABLE_NAME= tablename
            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return true;
        }
    }
}
