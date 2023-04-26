namespace aky.EmailService.Infrastructure.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using aky.EmailService.Domain.Dto;
    using aky.EmailService.Domain.Entities;
    using aky.EmailService.Domain.Repositories;
    using aky.EmailService.Domain.Services;

    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository templateRepository;

        public TemplateService(ITemplateRepository templateRepository)
        {
            this.templateRepository = templateRepository;
        }

        public async Task<TemplateDto> GetTemplateByCode(string code, string languageCode)
        {
            TemplateDto templateDto = null;
            Template template = await this.templateRepository.GetTemplateByCode(code, languageCode);

            if (template != null)
            {
                templateDto = new TemplateDto()
                {
                    EventCode = template.EventCode,
                    Name = template.Name,
                    Subject = template.Subject.FieldTexts.Where(ft => ft.Culture.LanguageCode == languageCode).Select(ft => ft.Value).FirstOrDefault(),
                    TemplatePath = template.TemplatePath.FieldTexts.Where(ft => ft.Culture.LanguageCode == languageCode).Select(ft => ft.Value).FirstOrDefault(),
                };
            }


            return templateDto;
        }
    }
}
