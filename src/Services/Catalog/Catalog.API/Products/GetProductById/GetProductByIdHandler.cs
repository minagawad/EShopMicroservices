using BuildingBlock.CQRS;
using Catalog.API.Models;
using Marten;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Product.GetProductById
{
    public record GetProductByIdQuery(Guid id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Models.Product Product);
    public class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {

            logger.LogInformation("GetProductByIdQueryHandler.Handle called with {@Query}", query);
            var product = await session.LoadAsync<Models.Product>(query.id, cancellationToken);
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            else
            {
                return new GetProductByIdResult(product);
            }
        }
    }
}
