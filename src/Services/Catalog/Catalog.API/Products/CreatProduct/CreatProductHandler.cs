using BuildingBlock.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Product.CreatProduct
{
    public record CreatProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price): ICommand<CreatProductResult>;
    public record CreatProductResult(Guid Id);
    public  class CreatProductCommandHandler(IDocumentSession session) : ICommandHandler<CreatProductCommand, CreatProductResult>
    {

        public async Task<CreatProductResult> Handle(CreatProductCommand command,  CancellationToken cancellationToken)
        {
            var product = new Models.Product()
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,

            };
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            return new CreatProductResult(product.Id);

        }
    }
}
