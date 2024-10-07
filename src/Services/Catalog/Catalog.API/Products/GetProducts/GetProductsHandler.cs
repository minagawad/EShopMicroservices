using BuildingBlock.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Product.GetProducts
{
    public record GetProductsResult(IEnumerable<Models.Product> Products);
    public record GeProuctsQuery() : IQuery<GetProductsResult>;
    public class GetProductsQueryHandler(IDocumentSession session,ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GeProuctsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GeProuctsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductQueryHanlder.Handel Called with {@Query}", query);
            var products= await session.Query<Product>().ToListAsync();
            return new GetProductsResult(products);
        }
    }
}
