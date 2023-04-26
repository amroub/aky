namespace Diatly.Foundation.Test.Repository
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Force.DeepCloner;
    using global::Diatly.Foundation.Repository.EF.Query;
    using global::Diatly.Foundation.Test.Domain;
    using global::Diatly.Foundation.Test.Repository.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public partial class TemplateRepositoryTest : BaseFixture
    {
        private ITemplateRepository templateRepository;

        public TemplateRepositoryTest()
        {
            this.templateRepository = this.TemplateRepository;
        }

        [Fact]
        public async Task TemplateRepositoryTest_FindAllAsync_UsingOneInclude_Pass()
        {
            var templatesToAdd = new Template()
            {
                EventCode = "FindAllAsyncTestWithOneInclude",
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

            var createdTemplates = await this.templateRepository.AddAsync(templatesToAdd);

            var includes = new Includes<Template>(query =>
            {
                return query.Include(t => t.Subject);
            });

            var resultUsingInclude = await this.templateRepository.FindAllAsync(a => a.EventCode == "FindAllAsyncTestWithOneInclude", includes);

            Assert.True(resultUsingInclude.Count == 1);

            foreach (var item in resultUsingInclude)
            {
                Assert.NotNull(item.Subject);
                Assert.Null(item.TemplatePath);
            }
        }

        [Fact]
        public async Task TemplateRepositoryTest_FindAllAsync_UsingNestedInclude_Pass()
        {
            var templatesToAdd = new Template()
            {
                EventCode = "FindAllAsyncTestWithNestedInclude",
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

            var createdTemplates = await this.templateRepository.AddAsync(templatesToAdd);

            var includes = new Includes<Template>(query =>
            {
                return query.Include(template => template.Subject).ThenInclude(field => field.FieldTexts).
                            Include(template => template.TemplatePath).ThenInclude(field => field.FieldTexts);
            });

            var resultUsingInclude = await this.templateRepository.FindAllAsync(a => a.EventCode == "FindAllAsyncTestWithNestedInclude", includes);

            Assert.True(resultUsingInclude.Count == 1, $"FinAll result count: ${resultUsingInclude.Count}");

            Assert.All(
                resultUsingInclude,
                item =>
                {
                    Assert.NotNull(item.Subject);
                    Assert.True(item.Subject.FieldTexts.Count > 0);

                    Assert.NotNull(item.TemplatePath);
                    Assert.True(item.TemplatePath.FieldTexts.Count > 0);
                });
        }

        [Theory]
        [MemberData(nameof(Templates))]
        public async Task TemplateRepositoryTest_GetAllAsync_UsingOneInclude_Pass(params Template[] templates)
        {
            var templatesToAdd = templates.DeepClone();

            var createdTemplates = await this.templateRepository.AddRangeAsync(templatesToAdd);

            var includes = new Includes<Template>(query =>
            {
                return query.Include(t => t.Subject);
            });

            var resultUsingInclude = await this.templateRepository.GetAllAsync(includes);

            Assert.True(resultUsingInclude.Count > 0);

            foreach (var item in resultUsingInclude)
            {
                Assert.NotNull(item.Subject);
                Assert.Null(item.TemplatePath);
            }
        }

        [Theory]
        [MemberData(nameof(Templates))]
        public async Task TemplateRepositoryTest_GetAllAsync_UsingNestedInclude_Pass(params Template[] templates)
        {
            var templatesToAdd = templates.DeepClone();

            var createdTemplates = await this.templateRepository.AddRangeAsync(templatesToAdd);

            var includes = new Includes<Template>(query =>
            {
                return query.Include(template => template.Subject).ThenInclude(field => field.FieldTexts).
                            Include(template => template.TemplatePath).ThenInclude(field => field.FieldTexts);
            });

            var resultUsingInclude = await this.templateRepository.GetAllAsync(includes);

            Assert.True(resultUsingInclude.Count > 0);

            Assert.All(
                resultUsingInclude,
                item =>
                {
                    Assert.NotNull(item.Subject);
                    Assert.True(item.Subject.FieldTexts.Count > 0);

                    Assert.NotNull(item.TemplatePath);
                    Assert.True(item.TemplatePath.FieldTexts.Count > 0);
                });
        }

        [Fact]
        public async Task TemplateRepositoryTest_FindAsync_UsingOneInclude_Pass()
        {
            var templatesToAdd = new Template()
            {
                EventCode = "FindAsyncWithOneInclude",
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

            var createdTemplates = await this.templateRepository.AddAsync(templatesToAdd);

            var includes = new Includes<Template>(query =>
            {
                return query.Include(t => t.Subject);
            });

            var resultUsingInclude = await this.templateRepository.FindAsync(a => a.EventCode == "FindAsyncWithOneInclude", includes);

            Assert.NotNull(resultUsingInclude);
        }

        public static IEnumerable<object[]> Templates
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
                new Template()
            {
                EventCode = "usercreated",
                Name = "User created template",
                Subject = new Field()
                {
                    Name = "SubjectId",
                    FieldTexts = new List<FieldText>
                    {
                        new FieldText()
                    {
                        Value = "user created subject in fr",
                        Culture = new Culture() { CultureCode = "fr-FR", CultureName = "French", LanguageCode = "fr" },
                    },
                        new FieldText()
                    {
                        Value = "user created subject in en",
                        Culture = new Culture() { CultureCode = "en-US", CultureName = "English", LanguageCode = "en" },
                    },
                        new FieldText()
                    {
                        Value = "user created subject in es",
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
                        Value = "user created template path in fr",
                        Culture = new Culture() { CultureCode = "fr-FR", CultureName = "French", LanguageCode = "fr" },
                    },
                        new FieldText()
                    {
                        Value = "user created template path in en",
                        Culture = new Culture() { CultureCode = "en-US", CultureName = "English", LanguageCode = "en" },
                    },
                        new FieldText()
                    {
                        Value = "user created template path in es",
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
