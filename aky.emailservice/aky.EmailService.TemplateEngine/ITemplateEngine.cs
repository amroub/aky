namespace aky.EmailService.TemplateEngine
{
    public interface ITemplateEngine
    {
        string TemplateDir { get; set; }

        string PrepareFromFile<T>(string templateFile, T data);

        string Prepare<T>(string liquidTemplate, T data);
    }
}
