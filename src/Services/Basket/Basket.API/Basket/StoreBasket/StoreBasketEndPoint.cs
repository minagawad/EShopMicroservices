using Basket.API.Models;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart Cart);
    public record StoreBasketResponse(string UserName);

    public class StoreBasketEndPoint: ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/basket", async (StoreBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>();
                var result = await sender.Send(command);
                var response = request.Adapt<StoreBasketResponse>();
                return Results.Created($"/basket/",response);
            })
            .WithName("StoreBasket")
            .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("creat store")
            .WithDescription("create store");
        }
    }
}
