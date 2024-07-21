
using Catalog.API.Models;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsResponse(IEnumerable<Product> Products);
    public class GetProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapGet("/Products", async (ISender sender) =>
            {
                var result = await sender.Send(new GeProuctsQuery());
                var response = result.Adapt<GetProductsResponse>();
                return Results.Ok(response);
            });

        }
    }
}
