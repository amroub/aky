namespace aky.EmailService.Domain.Entities
{
    using aky.Foundation.Ddd.Domain;

    public class Template
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string EventCode { get; set; }

        public int TemplatePathId { get; set; }

        public int SubjectId { get; set; }

        public virtual Field Subject { get; set; }

        public virtual Field TemplatePath { get; set; }
    }
}
