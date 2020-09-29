
namespace Patheyam.Web.Api.Controllers
{
    using Patheyam.Contract.Models;
    using Patheyam.Engine.Commands;
    using Patheyam.Engine.Queries;
    using Patheyam.Engine.Utils;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/v1/[controller]")]
    public sealed class TimeZonesController : BaseController
    {
        private readonly IMessages _messages;
        private readonly ILogger<TimeZonesController> _logger;
        public TimeZonesController(IMessages messages, ILogger<TimeZonesController> logger)
        {
            _logger = logger;
            _messages = messages;
        }
        [HttpGet]
        public async Task<IActionResult> GetTimeZonesAsync([FromQuery] SearchContract searchContract)
        {

            if (searchContract == null)
            {
                return Error("Invalid Input");
            }
            else if (searchContract.PageNumber < 1)
            {
                return Error("Invalid Page Number");
            }
            else if (searchContract.PageSize < 1)
            {
                return Error("Invalid Page Size");
            }

            _logger.LogInformation("GetTimeZonesAsync Called.");

            var query = new GetTimeZoneListQuery
            {
                SearchContract = searchContract
            };

            var list = await _messages.Dispatch(query).ConfigureAwait(false);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> AddTimeZoneAsync([FromBody] TimeZoneContract timeZone)
        {
            if (timeZone == null)
            {
                return Error("Invalid Payload");
            }
            else if (string.IsNullOrWhiteSpace(timeZone.Name))
            {
                return Error("Invalid time zone name.");
            }

            _logger.LogInformation("Adding time zone");

            timeZone.Id = 0;

            var command = new AddOrUpdateTimeZoneCommand
            {
                TimeZone = timeZone,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTimeZoneAsync([FromBody] TimeZoneContract timeZone)
        {
            if (timeZone == null)
            {
                return Error("Invalid Payload");
            }
            else if (timeZone.Id < 1)
            {
                return Error("Invalid time zone id");
            }
            else if (string.IsNullOrWhiteSpace(timeZone.Name))
            {
                return Error("Invalid time zone name");
            }

            _logger.LogInformation($"Updating time zone: ${timeZone.Id}");

            var command = new AddOrUpdateTimeZoneCommand
            {
                TimeZone = timeZone,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTimeZoneAsync(int timeZoneId)
        {
            if (timeZoneId < 1)
            {
                return Error("Invalid time zone Id");
            }
            var userId = GetUserIdFromClaim();
            _logger.LogInformation($"Deleting time zone : ${timeZoneId}, Requested By:${userId}");

            var command = new DeleteTimeZoneCommand
            {
                Id = timeZoneId,
                UserId = userId
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("deleteByIds")]
        public async Task<IActionResult> DeleteTimeZonesAsync([FromBody] List<int> idsList)
        {
            if (idsList == null)
            {
                return Error("Invalid Payload");
            }
            if (!idsList.Any())
            {
                return Error("Empty TimeZone Ids List");
            }
            var userId = GetUserIdFromClaim();
            _logger.LogInformation($"Deleting TimeZones: {string.Join(",", idsList)}, Requested By:{userId}");

            var command = new DeleteTimeZonesByIdsCommand
            {
                Ids = idsList,
                UserId = userId
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("updateStatusByIds/{status}")]
        public async Task<IActionResult> UpdateTimeZonesStatusByIdsAsync([FromBody] List<int> timeZoneIds, bool status)
        {
            if (timeZoneIds == null)
            {
                return Error("Invalid Payload");
            }
            else if (!timeZoneIds.Any())
            {
                return Error("Empty TimeZone IDs List");
            }

            _logger.LogInformation($"Updating TimeZones status for {string.Join(',', timeZoneIds)}");

            var command = new UpdateTimeZonesStatusByIdsCommand
            {
                Ids = timeZoneIds,
                Status = status,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }
    }
}