using BuildingBlock.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Product.DeleteProduct
{
    public record DeleteProductCommand(Guid Id): ICommand<DeletProductResult>;
    public record DeletProductResult(bool IsSuccess);
    public class DeleteProductCommandHandler (IDocumentSession session, ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeletProductResult>
    {
        public async Task<DeletProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteProductCommandHandler.Handle {@Command}", command);
            session.Delete <Models.Product > (command.Id);
            await session.SaveChangesAsync(cancellationToken);
            return new DeletProductResult(true);
        }
    }
}
