using Catalog.API.Models;

namespace Catalog.API.Product.GetProductByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<Models.Product> Products);

    public class GetProductByCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("products/category/{category}",async (string category, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category));
                var respons = result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(respons);
            }).WithName("GetProductsByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products By Category")
            .WithDescription("Get Products By Category");
        }
    }
}
