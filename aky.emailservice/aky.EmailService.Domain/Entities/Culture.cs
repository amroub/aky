namespace aky.EmailService.Domain.Entities
{
    using System.Collections.Generic;
    using aky.Foundation.Ddd.Domain;

    public class Culture : Entity
    {
        public string CultureName { get; set; }

        public string LanguageCode { get; set; }

        public string CultureCode { get; set; }

        public virtual List<FieldText> FieldTexts { get; set; }

        public Culture()
        {
            FieldTexts = new System.Collections.Generic.List<FieldText>();
        }
    }
}
