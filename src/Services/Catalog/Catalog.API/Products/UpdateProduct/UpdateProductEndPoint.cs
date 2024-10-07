﻿
using Catalog.API.Product.GetProductById;

namespace Catalog.API.Product.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    public record UpdateProductResponse(bool IsSuccess);
    public class UpdateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
          app.MapPut("/Products", async (UpdateProductRequest request,ISender sender) =>
          {
              var command= request.Adapt<UpdateProductCommand>();
              var result= sender.Send(command);
              var response= result.Adapt<UpdateProductResponse>();
              return Results.Ok(response);
          }).WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Product")
            .WithDescription("Update Product");

        }
    }
}
