
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
    public class productsController : BaseController
    {
        private readonly IMessages _messages;
        private readonly ILogger<productsController> _logger;
        public productsController(IMessages messages, ILogger<productsController> logger)
        {
            _logger = logger;
            _messages = messages;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync([FromQuery] SearchContract searchContract)
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

            _logger.LogInformation("GetproductsAsync Called.");

            var query = new GetProductsListQuery
            {
                SearchContract = searchContract
            };

            var list = await _messages.Dispatch(query).ConfigureAwait(false);
            return Ok(list);
        }

        [HttpGet]
        [Route("{productsId}")]
        public async Task<IActionResult> GetProductsAsync([FromRoute] int productsId)
        {
            if (productsId < 1)
            {
                return Error("Invalid Products Id");
            }

            _logger.LogInformation("GetProductsAsync Called.");

            var query = new GetProductsQuery
            {
                ProductsId = productsId
            };

            var list = await _messages.Dispatch(query).ConfigureAwait(false);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductsAsync([FromBody] ProductsContract products)
        {
            if (products == null)
            {
                return Error("Invalid Payload");
            }
           

            _logger.LogInformation("Adding Products");

            products.Id = 0;
            products.StatementType = "Insert";

            var command = new AddOrUpdateProductsCommand
            {
                Products = products,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductsAsync([FromBody] ProductsContract products)
        {
            if (products == null)
            {
                return Error("Invalid Payload");
            }
            else if (products.Id < 1)
            {
                return Error("Invalid products id");
            }           

            _logger.LogInformation($"Updating products: ${products.Id}");
            products.StatementType = "Update";
            var command = new AddOrUpdateProductsCommand
            {
                Products = products,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductsAsync(int productsId)
        {
            if (productsId < 1)
            {
                return Error("Invalid Products Id");
            }
            var userId = GetUserIdFromClaim();
            _logger.LogInformation($"Deleting Products: ${productsId}, Requested By:${userId}");

            ProductsContract products = new ProductsContract();
            products.StatementType = "Delete";
            products.Id = productsId;

            var command = new AddOrUpdateProductsCommand
            {
                Products = products,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("deleteByIds")]
        public async Task<IActionResult> DeleteProductsByIdsAsync([FromBody] List<int> idsList)
        {
            if (idsList == null)
            {
                return Error("Invalid Payload");
            }
            if (!idsList.Any())
            {
                return Error("Empty Products Ids List");
            }
            var userId = GetUserIdFromClaim();
            _logger.LogInformation($"Deleting Productss: {string.Join(",", idsList)}, Requested By:{userId}");

            ProductsContract products = new ProductsContract();
            products.StatementType = "Delete";

            var command = new AddOrUpdateProductsCommand
            {
                Products = products,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("updateStatusByIds/{status}")]
        public async Task<IActionResult> UpdateProductsStatusByIdsAsync([FromBody] List<int> productsIds, bool status)
        {
            if (productsIds == null)
            {
                return Error("Invalid Payload");
            }
            else if (!productsIds.Any())
            {
                return Error("Empty Products IDs List");
            }

            _logger.LogInformation($"Updating products status for {string.Join(',', productsIds)}");

            var command = new UpdateCompaniesStatusByIdsCommand
            {
                Ids = productsIds,
                Status = status,
                UserId = GetUserIdFromClaim()
            };
            var result = await _messages.Dispatch(command).ConfigureAwait(false);
            return Ok(result);
        }
    }
}