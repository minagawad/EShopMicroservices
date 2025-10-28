using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data
{
    public class CashedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await cache.RemoveAsync(userName, cancellationToken);
            return await basketRepository.DeleteBasket(userName, cancellationToken);

        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBaskey = await cache.GetStringAsync(userName, cancellationToken);
            if (string.IsNullOrEmpty(cachedBaskey))
            {
                var basket = await basketRepository.GetBasket(userName, cancellationToken);
                await cache.SetStringAsync(userName, System.Text.Json.JsonSerializer.Serialize(basket), cancellationToken);
                return basket;
            }
            return System.Text.Json.JsonSerializer.Deserialize<ShoppingCart>(cachedBaskey)!;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            await basketRepository.StoreBasket(cart, cancellationToken);

            await cache.SetStringAsync(cart.UserName, System.Text.Json.JsonSerializer.Serialize(cart), cancellationToken);
            return cart;
        }
    }
}
