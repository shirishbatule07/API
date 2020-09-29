
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
    public class CurrenciesController : BaseController
    {
        private readonly IMessages _messages;
        private readonly ILogger<CurrenciesController> _logger;
        public CurrenciesController(IMessages messages, ILogger<CurrenciesController> logger)
        {
            _logger = logger;
            _messages = messages;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrenciesAsync([FromQuery] SearchContract searchContract)
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

            _logger.LogInformation("GetCurrenciesAsync Called.");

            var query = new GetCurrencyListQuery
            {
                SearchContract = searchContract
            };

            var list = await _messages.Dispatch(query).ConfigureAwait(false);
            return Ok(list);
        }

        [HttpGet]
        [Route("{currencyId}")]
        public async Task<IActionResult> GetCurrencyAsync([FromRoute] int currencyId)
        {
            if (currencyId < 1)
            {
                return Error("Invalid Currency Id");
            }

            _logger.LogInformation("GetCurrencyAsync Called.");

            var query = new GetCurrencyQuery
            {
                CurrencyId = currencyId
            };

            var list = await _messages.Dispatch(query).ConfigureAwait(false);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> AddCurrencyAsync([FromBody] CurrencyContract currency)
        {
            if (currency == null)
            {
                return Error("Invalid Payload");
            }
            else if (string.IsNullOrWhiteSpace(currency.Code))
            {
                return Error("Invalid Currency Code");
            }

            _logger.LogInformation("Adding Currency");

            currency.Id = 0;

            var command = new AddOrUpdateCurrencyCommand
            {
                Currency = currency,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCurrencyAsync([FromBody] CurrencyContract currency)
        {
            if (currency == null)
            {
                return Error("Invalid Payload");
            }
            else if (currency.Id < 1)
            {
                return Error("Invalid currency id");
            }
            else if (string.IsNullOrWhiteSpace(currency.Code))
            {
                return Error("Invalid currency code");
            }

            _logger.LogInformation($"Updating currency: ${currency.Id}");

            var command = new AddOrUpdateCurrencyCommand
            {
                Currency = currency,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCurrencyAsync(int currencyId)
        {
            if (currencyId < 1)
            {
                return Error("Invalid Currency Id");
            }
            var userId = GetUserIdFromClaim();
            _logger.LogInformation($"Deleting Currency: ${currencyId}, Requested By:${userId}");

            var command = new DeleteCurrencyCommand
            {
                Id = currencyId,
                UserId = userId
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("deleteByIds")]
        public async Task<IActionResult> DeleteCurrenciesByIdsAsync([FromBody] List<int> idsList)
        {
            if (idsList == null)
            {
                return Error("Invalid Payload");
            }
            if (!idsList.Any())
            {
                return Error("Empty Currency Ids List");
            }
            var userId = GetUserIdFromClaim();
            _logger.LogInformation($"Deleting Currencys: {string.Join(",", idsList)}, Requested By:{userId}");

            var command = new DeleteCompaniesByIdsCommand
            {
                Ids = idsList,
                UserId = userId
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("updateStatusByIds/{status}")]
        public async Task<IActionResult> UpdateCurrenciesStatusByIdsAsync([FromBody] List<int> currencyIds, bool status)
        {
            if (currencyIds == null)
            {
                return Error("Invalid Payload");
            }
            else if (!currencyIds.Any())
            {
                return Error("Empty Currency IDs List");
            }

            _logger.LogInformation($"Updating Currencies status for {string.Join(',', currencyIds)}");

            var command = new UpdateCurrenciesStatusByIdsCommand
            {
                Ids = currencyIds,
                Status = status,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }
    }
}