using Catalog.API.Models;
using Marten;
using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();
            if (await session.Query<Catalog.API.Models.Product>().AnyAsync())
                return;
            session.Store<Catalog.API.Models.Product>(GetConfiguredProducts());
            await session.SaveChangesAsync();

        }

        private IEnumerable<Models.Product> GetConfiguredProducts()
        {
            return new List<Models.Product>()
           {
               new Models.Product
               {
                   Id= new Guid(),
                   Name="test",
                   Description="test",
                   ImageFile="image-test.jpg",
                   Price=100,
                   Category= new List<string> {"testCategory"}
               }
           };
        }
    }
}
