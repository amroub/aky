namespace aky.EmailService.Tests.Emails.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using aky.EmailService.Domain.Entities;
    using aky.EmailService.Domain.Repositories;
    using aky.Foundation.Repository.EF.Query;
    using Force.DeepCloner;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public partial class TemplateRepositoryTest : BaseFixture
    {
        private ITemplateRepository templateRepository;

        public TemplateRepositoryTest()
        {
            this.templateRepository = this.TemplateRepository;
        }

        [Theory]
        [MemberData(nameof(SingleTemplate))]
        public async Task TemplateRepository_AddAsync_Pass(Template template)
        {
            Template newTemplate = template.DeepClone();

            var entity = await this.templateRepository.AddAsync(newTemplate);

            Assert.NotNull(entity);
            Assert.True(entity.Id != 0);
            Assert.True(entity.Subject.FieldTexts.Count > 1); // subject is there in multiple languages
            Assert.True(entity.TemplatePath.FieldTexts.Count > 1); // TemplatePath field is there in multiple languages
        }

        [Theory]
        [MemberData(nameof(SingleTemplate))]
        public async Task TemplateRepository_UpdateAsync_Pass(Template template)
        {
            Template newTemplate = template.DeepClone();

            // add a new template first
            var entity = await this.templateRepository.AddAsync(newTemplate);

            // update newly added template with additional language for subject & template path
            entity.Subject.FieldTexts.Add(new FieldText() { Culture = new Culture() { CultureName = "Hindi", LanguageCode = "hi", CultureCode = "hi-HI" }, Value = "Subject Value in hindi" });
            entity.TemplatePath.FieldTexts.Add(new FieldText() { Culture = new Culture() { CultureName = "Hindi", LanguageCode = "hi", CultureCode = "hi-HI" }, Value = "Subject Value in hindi" });

            Assert.True(entity.Subject.FieldTexts.Where(a => a.Culture.LanguageCode == "hi").Count() > 0);
            Assert.True(entity.TemplatePath.FieldTexts.Where(a => a.Culture.LanguageCode == "hi").Count() > 0);
        }

        [Theory]
        [MemberData(nameof(SingleTemplate))]
        public async Task TemplateRepository_DeleteAsync_Pass(Template template)
        {
            Template newTemplate = template.DeepClone();

            // add a new template first
            var entity = await this.templateRepository.AddAsync(newTemplate);

            var result = await this.templateRepository.DeleteAsync(entity);

            Assert.True(result != 0);
        }

        [Theory]
        [MemberData(nameof(SingleTemplate))]
        public async Task TemplateRepository_GetAsync_Pass(Template template)
        {
            Template newTemplate = template.DeepClone();

            var entity = await this.templateRepository.AddAsync(newTemplate);

            var templateUsingGetAsync = await this.templateRepository.GetAsync(entity.Id);

            Assert.NotNull(templateUsingGetAsync);
            Assert.True(templateUsingGetAsync.Id != 0);
            Assert.True(templateUsingGetAsync.Subject.FieldTexts.Count > 1); // subject is there in multiple languages
            Assert.True(templateUsingGetAsync.TemplatePath.FieldTexts.Count > 1); // TemplatePath field is there in multiple languages
        }

        [Fact]
        public async Task TemplateRepository_GetTemplateByCode()
        {
            Template newTemplate = new Template()
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

            var entity = await this.templateRepository.AddAsync(newTemplate);

            var templateUsingGetAsync = await this.templateRepository.GetTemplateByCode("GetTemplateByCode", "fr");

            Assert.NotNull(templateUsingGetAsync);
        }

        [Theory]
        [MemberData(nameof(SingleTemplate))]
        public async Task TemplateRepository_GetTemplateByCode_WithNoTemplateForGivenCode(Template template)
        {
            Template newTemplate = template.DeepClone();

            var entity = await this.templateRepository.AddAsync(newTemplate);

            var templateUsingGetAsync = await this.templateRepository.GetTemplateByCode("unknown", "fr");

            Assert.Null(templateUsingGetAsync);
        }

        [Theory]
        [MemberData(nameof(SingleTemplate))]
        public async Task TemplateRepository_FindAsync_Pass(Template template)
        {
            Template newTemplate = template.DeepClone();

            newTemplate.Name = "template for find";

            var entity = await this.templateRepository.AddAsync(newTemplate);

            var includes = new Includes<Template>(query =>
            {
                return query.Include(t => t.Subject).ThenInclude(f => f.FieldTexts).
                                                        ThenInclude(ft => ft.Culture).
                            Include(t => t.TemplatePath).ThenInclude(f => f.FieldTexts).
                                                            ThenInclude(ft => ft.Culture);
            });

            var templateUsingFindAsync = await this.templateRepository.FindAsync(a => a.Name == newTemplate.Name, includes);

            Assert.NotNull(templateUsingFindAsync);
            Assert.True(templateUsingFindAsync.Id != 0);
            Assert.NotNull(templateUsingFindAsync.Subject);
            Assert.True(templateUsingFindAsync.Subject.FieldTexts.Count > 1); // subject is there in multiple languages
            Assert.NotNull(templateUsingFindAsync.TemplatePath);
            Assert.True(templateUsingFindAsync.TemplatePath.FieldTexts.Count > 1); // TemplatePath field is there in multiple languages
        }

        [Theory]
        [MemberData(nameof(SingleTemplate))]
        public async Task TemplateRepository_AddAsync_WithMissingLanguageText(Template template)
        {
            Template newTemplate = template.DeepClone();

            var entity = await this.templateRepository.AddAsync(newTemplate);

            Assert.NotNull(entity);
            Assert.True(entity.Id != 0);
            Assert.True(entity.Subject.FieldTexts.Where(a => a.Culture.LanguageCode == "hi").Count() == 0); // there is no subject text with languageCode = hi.
        }

        public static IEnumerable<object[]> SingleTemplate
        {
            get
            {
                yield return new Template[]
                {
                new Template()
            {
                EventCode = "resetpass",
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
            },
                };
            }
        }
    }
}
