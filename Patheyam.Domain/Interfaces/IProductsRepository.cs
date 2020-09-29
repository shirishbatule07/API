
namespace Patheyam.Domain.Interfaces
{
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductsRepository
    {
        Task<ProductsListDomain> GetProductsListAsync(SearchContract searchContract);
        Task<ProductsDomain> GetProductsAsync(int productsId);
        Task<int> AddOrUpdateProductsAsync(ProductsContract products, int userId);
        Task<bool> DeleteProductsAsync(int productsId, int userId);
        Task<SuccessFailureDomain> DeleteProductsByIdsAsync(List<int> productsIds, int userId);
        Task<bool> UpdateProductsStatusByIdsAsync(List<int> productsIds, int userId, bool status);
    }
}
