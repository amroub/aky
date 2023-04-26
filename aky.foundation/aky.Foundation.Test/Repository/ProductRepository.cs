namespace Diatly.Foundation.Test.Repository
{
    using global::Diatly.Foundation.Repository.EF;
    using global::Diatly.Foundation.Test.Domain;
    using global::Diatly.Foundation.Test.Repository.Interfaces;

    public class ProductRepository : AbstractRepository<Product>, IProductRepository
    {
        public ProductRepository(CatalogContext context)
        {
            this.context = context;
        }
    }
}
