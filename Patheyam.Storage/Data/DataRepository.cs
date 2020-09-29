
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

    public class DataRepository : IDataRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public DataRepository(IConnectionFactory connectionFactory)
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
    }
}
