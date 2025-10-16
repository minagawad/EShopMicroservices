using Basket.API.Data;
using BuildingBlock.CQRS;

namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string UserName) : IQuery<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsDeleted);
    public class DeleteBasketHandler(IBasketRepository basketRepository) : IQueryHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand query, CancellationToken cancellationToken)
        {
            await basketRepository.DeleteBasket(query.UserName, cancellationToken);

            return new DeleteBasketResult(true);
        }
    }



}
