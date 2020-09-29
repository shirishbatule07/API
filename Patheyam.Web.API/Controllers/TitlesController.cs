
namespace Patheyam.Web.Api.Controllers
{
    using Patheyam.Contract.Models;
    using Patheyam.Engine.Commands;
    using Patheyam.Engine.Queries;
    using Patheyam.Engine.Utils;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class TitlesController : BaseController
    {
        private readonly IMessages _messages;
        private readonly ILogger<TitlesController> _logger;
        public TitlesController(IMessages messages, ILogger<TitlesController> logger)
        {
            _logger = logger;
            _messages = messages;
        }

        [HttpGet]
        public async Task<IActionResult> GetTitlesAsync([FromQuery] SearchContract searchContract)
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

            _logger.LogInformation("GetTitlesAsync Called.");

            var query = new GetTitleListQuery
            {
                SearchContract = searchContract
            };

            var list = await _messages.Dispatch(query).ConfigureAwait(false);
            return Ok(list);
        }
    }
}