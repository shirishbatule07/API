
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
    public sealed class LocationsController : BaseController
    {
        private readonly IMessages _messages;
        private readonly ILogger<LocationsController> _logger;
        public LocationsController(IMessages messages, ILogger<LocationsController> logger)
        {
            _logger = logger;
            _messages = messages;
        }

        [HttpGet]
        [Route("Cities")]
        public async Task<IActionResult> GetCitiesAsync([FromQuery] SearchContract searchContract)
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

            _logger.LogInformation("GetCitiesAsync Called.");

            var query = new GetCityListQuery
            {
                SearchContract = searchContract
            };

            var list = await _messages.Dispatch(query).ConfigureAwait(false);
            return Ok(list);
        }

        [HttpGet]
        [Route("States")]
        public async Task<IActionResult> GetStatesAsync([FromQuery] SearchContract searchContract)
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

            _logger.LogInformation("GetStatesAsync Called.");

            var query = new GetStateListQuery
            {
                SearchContract = searchContract
            };

            var list = await _messages.Dispatch(query).ConfigureAwait(false);
            return Ok(list);
        }


        [HttpGet]
        [Route("Countries")]
        public async Task<IActionResult> GetCountriesAsync([FromQuery] SearchContract searchContract)
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

            _logger.LogInformation("GetCountriesAsync Called.");

            var query = new GetCountryListQuery
            {
                SearchContract = searchContract
            };

            var list = await _messages.Dispatch(query).ConfigureAwait(false);
            return Ok(list);
        }

        [HttpGet]
        [Route("City/{cityId}")]
        public async Task<IActionResult> GetLocationByCityIdAsync(int cityId)
        {
            if (cityId < 0)
            {
                return Error("Invalid City Id");
            }

            _logger.LogInformation("GetLocationByCityIdAsync Called.");

            var query = new GetLocationByCityIdQuery
            {
                CityId = cityId
            };

            var result = await _messages.Dispatch(query).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
