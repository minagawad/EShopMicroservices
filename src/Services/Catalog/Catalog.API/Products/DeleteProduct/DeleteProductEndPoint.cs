
using Catalog.API.Product.DeleteProduct;
using Catalog.API.Product.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid Id);
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndPoint  : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
           app.MapDelete("/Products/{id}" ,async(Guid id, ISender sender) =>
           {
            var result=   await sender.Send(new DeleteProductCommand(id));
               var response = result.Adapt<DeleteProductResponse>();

               return  Results.Ok(response);

           }).WithName("DeleteProduct")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Product")
            .WithDescription("Delete Product");
        }
    }
}
