using Marten;
using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            // Check if any product exists
            if (await session.Query<Catalog.API.Models.Product>().AnyAsync(cancellation))
                return;

            // Add initial products
            var products = GetConfiguredProducts();
            session.Store(products);

            // Save changes
            await session.SaveChangesAsync(cancellation);
        }

        private IEnumerable<Models.Product> GetConfiguredProducts()
        {
            return new List<Models.Product>
        {
            new Models.Product
            {
                Id = Guid.NewGuid(), // Generate a unique GUID
                Name = "Test Product",
                Description = "This is a test product.",
                ImageFile = "image-test.jpg",
                Price = 100,
                Category = new List<string> { "TestCategory" }
            }
        };
        }
    }
}