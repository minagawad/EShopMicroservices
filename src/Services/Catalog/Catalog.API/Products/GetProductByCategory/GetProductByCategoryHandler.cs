using BuildingBlock.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Product.GetProductByCategory
{
    public record GetProductByCategoryQuery(string category) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Models.Product> Products);
    public class GetProductByCategoryHandler (IDocumentSession session, ILogger<GetProductByCategoryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByCategoryHandler.Handle called with {@Query}", query);
            var products =await session.Query<Models.Product>()
                .Where(p => p.Category.Contains(query.category))
                .ToListAsync(cancellationToken); ;

            return new GetProductByCategoryResult(products);
        }
    }
}
