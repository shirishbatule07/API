
namespace Patheyam.Engine.Queries
{
    using Patheyam.Common;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Domain.Models;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;

    public sealed class GetProductsQuery : IQuery<ProductsDomain>
    {
        public int ProductsId { get; set; }
    }

    [AuditLog]
    public sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, ProductsDomain>
    {
        private readonly IProductsRepository _productsRepository;
        public GetProductsQueryHandler(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<ProductsDomain> Handle(GetProductsQuery query)
        {
            query.ThrowIfNull("Invalid query parameter", nameof(query));
            query.ProductsId.ThrowIfNotPositiveNonZeroInt("Invalid Products Id", nameof(query.ProductsId));
            return await _productsRepository.GetProductsAsync(query.ProductsId).ConfigureAwait(false);
        }
    }
}
