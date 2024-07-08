
namespace Catalog.API.Products.CreatProduct
{

    public record CreatProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    public record CreatProductRespnse(Guid Id);
    public class CreatProuctEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/Products", async (CreatProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreatProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreatProductRespnse>();
                return Results.Created($"/Products/{response.Id}", response);

            })
            .WithName("CreatProduct")
            .Produces<CreatProductRespnse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Creat Product")
            .WithDescription("Crat Product");
        }
    }

}
