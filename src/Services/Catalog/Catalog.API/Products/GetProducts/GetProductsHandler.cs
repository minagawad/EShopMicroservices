using BuildingBlock.CQRS;
using Marten;


namespace Catalog.API.Product.GetProducts
{
    public record GetProductsResult(IEnumerable<Models.Product> Products);
    public record GeProuctsQuery(int PageNumer = 1, int PageSize = 10) : IQuery<GetProductsResult>;
    public class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GeProuctsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GeProuctsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductQueryHanlder.Handel Called with {@Query}", query);
            var products = await session.Query<Models.Product>().ToPagedListAsync(query.PageNumer, query.PageSize, cancellationToken);
            return new GetProductsResult(products);
        }
    }
}
