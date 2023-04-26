using System.Collections.Generic;

namespace Akeneo.Model
{
    public class FamilyVariant : ModelBase
    {
        public string Code { get; set; }
        public List<Variant> VariantAttributeSets { get; set; }
        public Dictionary<string, string> Labels { get; set; }
    }

}