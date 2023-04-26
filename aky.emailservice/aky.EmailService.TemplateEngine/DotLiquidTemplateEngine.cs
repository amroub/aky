namespace aky.EmailService.TemplateEngine
{
    using System;
    using System.IO;
    using DotLiquid;

    public class DotLiquidTemplateEngine : ITemplateEngine
    {
        public string TemplateDir { get; set; }

        public string Prepare<T>(string liquidTemplate, T data)
        {
            return this.RenderTemplate(liquidTemplate, data);
        }

        public string PrepareFromFile<T>(string templateFile, T data)
        {
            string content = string.Empty;

            templateFile = templateFile.Contains(@"\")
                                ? templateFile
                                : $@"{this.TemplateDir}\{templateFile}";

            if (File.Exists(templateFile))
            {
                var liquidTemplate = File.ReadAllText(templateFile);

                content = this.RenderTemplate(templateFile, data);
            }
            else
            {
                throw new FileNotFoundException($"Templatefile {templateFile} not found.");
            }

            return content;
        }

        private string RenderTemplate<T>(string liquidTemplate, T data)
        {
            LiquidFunctions.RegisterViewModel(typeof(T));

            var template = Template.Parse(liquidTemplate);

            var body = template.RenderViewModel(data);

            return body;
        }
    }
}
