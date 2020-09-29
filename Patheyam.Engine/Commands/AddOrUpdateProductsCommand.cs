
namespace Patheyam.Engine.Commands
{
    using Patheyam.Common;
    using Patheyam.Contract.Models;
    using Patheyam.Domain.Interfaces;
    using Patheyam.Engine.Utils;
    using System.Threading.Tasks;


    public sealed class AddOrUpdateProductsCommand : ICommand<int>
    {
        public ProductsContract Products { get; set; }
        public int UserId { get; set; }
    }

    [AuditLog]
    public sealed class AddOrUpdateProductsCommandHandler : ICommandHandler<AddOrUpdateProductsCommand, int>
    {
        private readonly IProductsRepository _productsRepository;
        public AddOrUpdateProductsCommandHandler(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<int> Handle(AddOrUpdateProductsCommand command)
        {
            
            command.Products.ProductName.ThrowIfNullOrEmpty("Invalid products name parameter", nameof(command.Products.ProductName));
            command.UserId.ThrowIfNotPositiveNonZeroInt("Invalid user id parameter", nameof(command.UserId));
            return await _productsRepository.AddOrUpdateProductsAsync(command.Products, command.UserId).ConfigureAwait(false);
        }
    }
}
