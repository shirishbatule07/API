
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
    public class CommanController : BaseController
    {
        private readonly IMessages _messages;
        private readonly ILogger<CommanController> _logger;
        public CommanController(IMessages messages, ILogger<CommanController> logger)
        {
            _logger = logger;
            _messages = messages;
        }

        [HttpPost("updateStatusByIds/{status}")]
        public async Task<IActionResult> UpdateStatusByIdsAsync([FromBody] List<int> Ids, bool status, string tablename)
        {
            if (Ids == null)
            {
                return Error("Invalid Payload");
            }
            else if (!Ids.Any())
            {
                return Error("Empty  IDs List");
            }

            _logger.LogInformation($"Updating  status for {string.Join(',', Ids)}");

            var command = new UpdateStatusByIdsCommand
            {
                Ids = Ids,
                Status = status,
                UserId = GetUserIdFromClaim(),
                Tablename=tablename
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }
    }
}