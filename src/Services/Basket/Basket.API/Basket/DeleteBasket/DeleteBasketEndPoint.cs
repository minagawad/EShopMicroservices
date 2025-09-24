using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketResponse(bool IsSuccess);


    public class DeleteBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketCommand(userName));
                var response = result.Adapt<DeleteBasketResponse>();
                return Results.Ok(response);

            })
             .WithName("DeleteProduct")
             .Produces<DeleteBasketResponse>(statusCode: StatusCodes.Status200OK)
             .WithSummary(" Delete product");
        }
    }
}
