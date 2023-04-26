namespace Diatly.Foundation.Test.Domain
{
    using System.Collections.Generic;
    using global::Diatly.Foundation.Ddd.Domain;

    public class Field : Entity
    {
        public string Name { get; set; }

        public virtual List<FieldText> FieldTexts { get; set; }

        public virtual List<Template> TemplatesSubjectId { get; set; }

        public virtual List<Template> TemplatesTemplatePathId { get; set; }

        public Field()
        {
            this.FieldTexts = new System.Collections.Generic.List<FieldText>();
            this.TemplatesTemplatePathId = new System.Collections.Generic.List<Template>();
            this.TemplatesSubjectId = new System.Collections.Generic.List<Template>();
        }
    }
}
