
namespace Patheyam.Storage.Data
{
    using Dapper;
    using Patheyam.Common;
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductsRepository : IProductsRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public ProductsRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> AddOrUpdateProductsAsync(ProductsContract products, int userId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.spProductsInsertUpdateDelete;
            var result = await connection.QueryAsync<int>(procName, new
            {
                Product_id =products.Id,
                Company_id= products.Companyid,
                Product_name=products.ProductName,
                Techname= products.Techname,
                Searchname= products.Searchname,
                Description= products.Description,
                PrimeImage=products.PrimeImage,
                Price= products.Price,
                Sold_By= products.SoldBy,
                Specifications=products.Specifications,
                ProductURL= products.ProductURL,
                AvgCustReview= products.AvgCustReview,
                YrofLaunch=  products.YrofLaunch,
                StatementType= products.StatementType

            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public async Task<SuccessFailureDomain> DeleteProductsByIdsAsync(List<int> productsIds, int userId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.spProductsInsertUpdateDelete;
            var result = await connection.QueryMultipleAsync(procName, new
            {
                Ids = productsIds.ConvertToDataTable("Value"),
                UserId = userId
            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            var failedIds = await result.ReadAsync<int>().ConfigureAwait(false);
            return new SuccessFailureDomain
            {
                SuccessIds = productsIds.Where(ids => !failedIds.Contains(ids)).ToList(),
                FailureIds = failedIds.ToList()
            };
        }

        public async Task<bool> DeleteProductsAsync(int productsId, int userId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.spProductsInsertUpdateDelete;
            await connection.QueryAsync(procName, new
            {
                Id = productsId,
                UserId = userId
            }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return true;
        }

        public async Task<ProductsDomain> GetProductsAsync(int productsId)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.spMaster_GetProductById;
            var result = await connection.QuerySingleAsync<ProductsDomain>(procName, new { Id = productsId }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return result;
        }

        public async Task<ProductsListDomain> GetProductsListAsync(SearchContract searchContract)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.spMaster_GetProducts;
            var result = await connection.QueryMultipleAsync(procName, searchContract, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            var currencies = await result.ReadAsync<ProductsDomain>().ConfigureAwait(false);
            var pagination = await result.ReadAsync<PaginationInfo>().ConfigureAwait(false);
            return new ProductsListDomain
            {
                Products = currencies.ToList(),
                Pagination = pagination.FirstOrDefault()
            };
        }

        public async Task<bool> UpdateProductsStatusByIdsAsync(List<int> productsIds, int userId, bool status)
        {
            using var connection = _connectionFactory.GetDbConnection();
            var procName = StoredProcedureConstants.spProductsInsertUpdateDelete;
            await connection.QueryAsync(procName, new { Ids = productsIds.ConvertToDataTable("Value"), Status = status, UserId = userId }, null, null, CommandType.StoredProcedure).ConfigureAwait(false);
            return true;
        }
    }
}
