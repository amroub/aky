namespace aky.EmailService.Infrastructure.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using aky.EmailService.Domain.Dto;
    using aky.EmailService.Domain.Entities;
    using aky.EmailService.Domain.Repositories;
    using aky.Foundation.Repository.EF;
    using aky.Foundation.Repository.EF.Query;
    using Microsoft.EntityFrameworkCore;
    using Ordering.Infrastructure;

    public class TemplateRepository : AbstractRepository<Template>, ITemplateRepository
    {
        public TemplateRepository(EmailDbContext context)
        {
            this.context = context;
        }

        public async Task<Template> GetTemplateByCode(string code, string languageCode)
        {
            var includes = new Includes<Template>(query =>
            {
                return query.Include(t => t.Subject).ThenInclude(f => f.FieldTexts).
                                                        ThenInclude(ft => ft.Culture).
                            Include(t => t.TemplatePath).ThenInclude(f => f.FieldTexts).
                                                            ThenInclude(ft => ft.Culture);
            });

            return await this.FindAsync(template => template.EventCode == code, includes);
        }
    }
}
