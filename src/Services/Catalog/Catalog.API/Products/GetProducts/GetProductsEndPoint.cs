namespace Catalog.API.Product.GetProducts
{
    public record GetProductRequest(int? pageNumber = 1, int pageSize = 10);
    public record GetProductsResponse(IEnumerable<Models.Product> Products);
    public class GetProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapGet("/Products", async ([AsParameters] GetProductRequest request, ISender sender) =>
            {
                var query = request.Adapt<GeProuctsQuery>();
                var result = await sender.Send(query);
                var response = result.Adapt<GetProductsResponse>();
                return Results.Ok(response);
            })
             .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Producst")
              ;

        }
    }
}
