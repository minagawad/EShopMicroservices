using BuildingBlock.CQRS;

namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string UserName) : IQuery<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsDeleted);
    public class DeleteBasketHandler : IQueryHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public Task<DeleteBasketResult> Handle(DeleteBasketCommand query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }



}
