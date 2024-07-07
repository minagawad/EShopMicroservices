using MediatR;

namespace Catalog.API.Products.CreatProduct
{
    public record CreatProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price): IRequest<CreatProductResult>;
    public record CreatProductResult(Guid Id);
    public class CreatProductCommandHandler : IRequestHandler<CreatProductCommand, CreatProductResult>
    {
        public Task<CreatProductResult> Handle(CreatProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
