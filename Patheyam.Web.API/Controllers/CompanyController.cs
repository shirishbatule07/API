
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
    public class companiesController : BaseController
    {
        private readonly IMessages _messages;
        private readonly ILogger<companiesController> _logger;
        public companiesController(IMessages messages, ILogger<companiesController> logger)
        {
            _logger = logger;
            _messages = messages;
        }

        [HttpGet]
        public async Task<IActionResult> GetcompaniesAsync([FromQuery] SearchContract searchContract)
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

            _logger.LogInformation("GetcompaniesAsync Called.");

            var query = new GetCompanyListQuery
            {
                SearchContract = searchContract
            };

            var list = await _messages.Dispatch(query).ConfigureAwait(false);
            return Ok(list);
        }

        [HttpGet]
        [Route("{companyId}")]
        public async Task<IActionResult> GetCompanyAsync([FromRoute] int companyId)
        {
            if (companyId < 1)
            {
                return Error("Invalid Company Id");
            }

            _logger.LogInformation("GetCompanyAsync Called.");

            var query = new GetCompanyQuery
            {
                CompanyId = companyId
            };

            var list = await _messages.Dispatch(query).ConfigureAwait(false);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanyAsync([FromBody] CompanyContract company)
        {
            if (company == null)
            {
                return Error("Invalid Payload");
            }
           

            _logger.LogInformation("Adding Company");

            company.Id = 0;

            var command = new AddOrUpdateCompanyCommand
            {
                Company = company,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCompanyAsync([FromBody] CompanyContract company)
        {
            if (company == null)
            {
                return Error("Invalid Payload");
            }
            else if (company.Id < 1)
            {
                return Error("Invalid company id");
            }
           

            _logger.LogInformation($"Updating company: ${company.Id}");

            var command = new AddOrUpdateCompanyCommand
            {
                Company = company,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCompanyAsync(int companyId)
        {
            if (companyId < 1)
            {
                return Error("Invalid Company Id");
            }
            var userId = GetUserIdFromClaim();
            _logger.LogInformation($"Deleting Company: ${companyId}, Requested By:${userId}");

            var command = new DeleteCompanyCommand
            {
                Id = companyId,
                UserId = userId
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("deleteByIds")]
        public async Task<IActionResult> DeletecompaniesByIdsAsync([FromBody] List<int> idsList)
        {
            if (idsList == null)
            {
                return Error("Invalid Payload");
            }
            if (!idsList.Any())
            {
                return Error("Empty Company Ids List");
            }
            var userId = GetUserIdFromClaim();
            _logger.LogInformation($"Deleting Companys: {string.Join(",", idsList)}, Requested By:{userId}");

            var command = new DeleteCompaniesByIdsCommand
            {
                Ids = idsList,
                UserId = userId
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("updateStatusByIds/{status}")]
        public async Task<IActionResult> UpdatecompaniesStatusByIdsAsync([FromBody] List<int> companyIds, bool status)
        {
            if (companyIds == null)
            {
                return Error("Invalid Payload");
            }
            else if (!companyIds.Any())
            {
                return Error("Empty Company IDs List");
            }

            _logger.LogInformation($"Updating companies status for {string.Join(',', companyIds)}");

            var command = new UpdateCompaniesStatusByIdsCommand
            {
                Ids = companyIds,
                Status = status,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }
    }
}