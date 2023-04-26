using Diatly.Foundation.Ddd.Domain;

namespace Diatly.Foundation.Test.Domain
{
    public class FieldText : Entity
    {
        public int FieldId { get; set; }

        public string Value { get; set; }

        public int CultureId { get; set; }

        public virtual Culture Culture { get; set; }

        public virtual Field Field { get; set; }
    }
}
