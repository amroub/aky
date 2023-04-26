namespace aky.EmailService.Tests.Emails.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using aky.EmailService.Domain.Dto;
    using aky.EmailService.Domain.Entities;
    using aky.EmailService.Domain.Repositories;
    using aky.EmailService.Domain.Services;
    using aky.EmailService.Infrastructure.Services;
    using Moq;
    using Xunit;

    public class TemplateServiceTest : BaseFixture
    {
        public TemplateServiceTest()
        {
        }

        [Fact]
        public async Task TemplateService_GetTemplateByCode_Pass()
        {
            Template fakeTemplate = new Template()
            {
                EventCode = "GetTemplateByCode",
                Name = "Reset password email template",
                Subject = new Field()
                {
                    Name = "SubjectId",
                    FieldTexts = new List<FieldText>
                    {
                        new FieldText()
                    {
                        Value = "subject in fr",
                        Culture = new Culture() { CultureCode = "fr-FR", CultureName = "French", LanguageCode = "fr" },
                    },
                        new FieldText()
                    {
                        Value = "subject in en",
                        Culture = new Culture() { CultureCode = "en-US", CultureName = "English", LanguageCode = "en" },
                    },
                        new FieldText()
                    {
                        Value = "subject in es",
                        Culture = new Culture() { CultureCode = "es-ES", CultureName = "Spanish", LanguageCode = "es" },
                    },
                },
                },
                TemplatePath = new Field()
                {
                    Name = "TemplatePathId",
                    FieldTexts = new List<FieldText>
                    {
                        new FieldText()
                    {
                        Value = "template path in fr",
                        Culture = new Culture() { CultureCode = "fr-FR", CultureName = "French", LanguageCode = "fr" },
                    },
                        new FieldText()
                    {
                        Value = "template path in en",
                        Culture = new Culture() { CultureCode = "en-US", CultureName = "English", LanguageCode = "en" },
                    },
                        new FieldText()
                    {
                        Value = "template path in es",
                        Culture = new Culture() { CultureCode = "es-ES", CultureName = "Spanish", LanguageCode = "es" },
                    },
                },
                },
            };

            Mock<ITemplateRepository> templateRepository = new Mock<ITemplateRepository>();

            templateRepository.Setup(a => a.GetTemplateByCode(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(fakeTemplate);

            ITemplateService templateService = new TemplateService(templateRepository.Object);

            var template = await templateService.GetTemplateByCode(It.IsAny<string>(), It.IsAny<string>());

            Assert.NotNull(template);
        }

        [Fact]
        public async Task TemplateService_GetTemplateByCode_WithNoTemplateForGivenCode()
        {
            Template fakeTemplate = null;
            Mock<ITemplateRepository> templateRepository = new Mock<ITemplateRepository>();

            templateRepository.Setup(a => a.GetTemplateByCode(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(fakeTemplate);

            ITemplateService templateService = new TemplateService(templateRepository.Object);

            var template = await templateService.GetTemplateByCode(It.IsAny<string>(), It.IsAny<string>());

            Assert.Null(template);
        }

        [Fact]
        public async Task TemplateService_GetTemplateByCode_TemplateWithMissingLanguageSpecificContent()
        {
            Template fakeTemplate = new Template()
            {
                EventCode = "TemplateWithMissingLanguageSpecificContent",
                Name = "Reset password email template",
                Subject = new Field()
                {
                    Name = "SubjectId",
                    FieldTexts = new List<FieldText>
                    {
                        new FieldText()
                    {
                        Value = "subject in fr",
                        Culture = new Culture() { CultureCode = "fr-FR", CultureName = "French", LanguageCode = "fr" },
                    },
                        new FieldText()
                    {
                        Value = "subject in en",
                        Culture = new Culture() { CultureCode = "en-US", CultureName = "English", LanguageCode = "en" },
                    },
                        new FieldText()
                    {
                        Value = "subject in es",
                        Culture = new Culture() { CultureCode = "es-ES", CultureName = "Spanish", LanguageCode = "es" },
                    },
                },
                },
                TemplatePath = new Field()
                {
                    Name = "TemplatePathId",
                    FieldTexts = new List<FieldText>
                    {
                        new FieldText()
                    {
                        Value = "template path in fr",
                        Culture = new Culture() { CultureCode = "fr-FR", CultureName = "French", LanguageCode = "fr" },
                    },
                        new FieldText()
                    {
                        Value = "template path in en",
                        Culture = new Culture() { CultureCode = "en-US", CultureName = "English", LanguageCode = "en" },
                    },
                        new FieldText()
                    {
                        Value = "template path in es",
                        Culture = new Culture() { CultureCode = "es-ES", CultureName = "Spanish", LanguageCode = "es" },
                    },
                },
                },
            };

            Mock<ITemplateRepository> templateRepository = new Mock<ITemplateRepository>();

            templateRepository.Setup(a => a.GetTemplateByCode(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(fakeTemplate);

            ITemplateService templateService = new TemplateService(templateRepository.Object);

            var template = await templateService.GetTemplateByCode(It.IsAny<string>(), "hi");

            Assert.NotNull(template);
            Assert.True(string.IsNullOrEmpty(template.Subject));
        }
    }
}
