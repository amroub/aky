namespace aky.EmailService.Domain.Services
{
    using System.Threading.Tasks;
    using aky.EmailService.Domain.Dto;

    public interface ITemplateService
    {
        Task<TemplateDto> GetTemplateByCode(string code, string languageCode);
    }
}
