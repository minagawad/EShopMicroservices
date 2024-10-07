
using Catalog.API.Models;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Product.GetProducts
{
    public record GetProductsResponse(IEnumerable<Models.Product> Products);
    public class GetProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapGet("/Products", async (ISender sender) =>
            {
                var result = await sender.Send(new GeProuctsQuery());
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
