
namespace Patheyam.Storage.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Patheyam.Domain.Interfaces;
    using Microsoft.Extensions.Configuration;

    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionStringName = "DefaultConnection";

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetDbConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        private string GetConnectionString()
        {
            return _configuration?.GetSection("AppSettings")?.GetSection(_connectionStringName)?.Value ??
                   throw new InvalidOperationException($"{_connectionStringName} could not be found in the config.");
        }
    }
}
