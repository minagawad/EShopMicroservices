using BuildingBlock.CQRS;

namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketQuery(string UserName) : IQuery<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsDeleted);
    public class DeleteBasketHandler : IQueryHandler<DeleteBasketQuery, DeleteBasketResult>
    {
        public Task<DeleteBasketResult> Handle(DeleteBasketQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }



}
