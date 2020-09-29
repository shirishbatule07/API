
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

    public class TimeZoneRepository : ITimeZoneRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public TimeZoneRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<TimeZoneListDomain> GetTimeZoneListAsync(SearchContract searchContract)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.GetTimeZones;
            var result = await connection.QueryMultipleAsync(procName, searchContract, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            var timeZones = await result.ReadAsync<TimeZoneDomain>().ConfigureAwait(false);
            var pagination = await result.ReadAsync<PaginationInfo>().ConfigureAwait(false);
            return new TimeZoneListDomain
            {
                TimeZones = timeZones.ToList(),
                Pagination = pagination.FirstOrDefault()
            };
        }

        public async Task<int> AddOrUpdateTimeZoneAsync(TimeZoneContract timeZone, int userId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.UpsertTimeZone;
            var result = await connection.QueryAsync<int>(procName, new
            {
                timeZone.Id,
                timeZone.Name,
                timeZone.Description,
                timeZone.Hours,
                UserId = userId,
                timeZone.Active

            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public async Task<bool> DeleteTimeZoneAsync(int timeZoneId, int userId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.DeleteTimeZone;
            await connection.QueryAsync(procName, new
            {
                Id = timeZoneId,
                UserId = userId
            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return true;
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

        public async Task<SuccessFailureDomain> DeleteTimeZonesByIdsAsync(List<int> timeZoneIds, int userId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.DeleteTimeZonesByIds;
            var result = await connection.QueryMultipleAsync(procName, new
            {
                Ids = timeZoneIds.ConvertToDataTable("Value"),
                UserId = userId
            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            var failedIds = await result.ReadAsync<int>().ConfigureAwait(false);
            return new SuccessFailureDomain
            {
                SuccessIds = timeZoneIds.Where(ids => !failedIds.Contains(ids)).ToList(),
                FailureIds = failedIds.ToList()
            };
        }

        public async Task<bool> UpdateTimeZonesStatusByIdsAsync(List<int> timeZoneIds, int userId, bool status)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.UpdateTimeZonesStatusByIds;
            await connection.QueryAsync(procName, new { Ids = timeZoneIds.ConvertToDataTable("Value"), Status = status, UserId = userId }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return true;
        }
    }
}
