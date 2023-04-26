namespace aky.EmailService.Domain.Repositories
{
    using System.Threading.Tasks;
    using aky.EmailService.Domain.Dto;
    using aky.EmailService.Domain.Entities;
    using aky.Foundation.Repository;

    public interface ITemplateRepository : IRepository<Template>
    {
        Task<Template> GetTemplateByCode(string code, string languageCode);
    }
}
