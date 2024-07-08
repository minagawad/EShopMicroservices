using BuildingBlock.CQRS;
using Catalog.API.Models;
using MediatR;

namespace Catalog.API.Products.CreatProduct
{
    public record CreatProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price): ICommand<CreatProductResult>;
    public record CreatProductResult(Guid Id);
    public  class CreatProductCommandHandler : ICommandHandler<CreatProductCommand, CreatProductResult>
    {
        public async Task<CreatProductResult> Handle(CreatProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product()
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,

            };
            return new CreatProductResult(Guid.NewGuid());

        }
    }
}
