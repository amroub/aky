namespace Diatly.Foundation.Test.Domain
{
    using global::Diatly.Foundation.Ddd.Domain;
    using System.Collections.Generic;

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
