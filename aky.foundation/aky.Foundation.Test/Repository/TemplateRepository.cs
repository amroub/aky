namespace Diatly.Foundation.Test.Repository
{
    using global::Diatly.Foundation.Repository.EF;
    using global::Diatly.Foundation.Test.Domain;
    using global::Diatly.Foundation.Test.Repository.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class TemplateRepository : AbstractRepository<Template>, ITemplateRepository
    {
        public TemplateRepository(CatalogContext context)
        {
            this.context = context;
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
