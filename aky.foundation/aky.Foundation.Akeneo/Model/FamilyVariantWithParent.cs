namespace Akeneo.Model
{
    public class FamilyVariantWithParent : FamilyVariant
    {
        public string ParentCode { get; set; } = string.Empty;

        public FamilyVariant ToFamilyVariant()
        {
            return new FamilyVariant
            {
                Code = this.Code,
                Labels = this.Labels,
                VariantAttributeSets = this.VariantAttributeSets
            };
        }
    }
}